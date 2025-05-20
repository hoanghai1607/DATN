import { Col, Container, Row } from "react-bootstrap";
import BoardCard from "../../Components/common/BoardCard";
import BoardTitle from "../../Components/common/BoardTitle";
import CreateBoard from "../../Components/common/CreateBoard";
import WorkSpacesTitle from "../../Components/common/WorkSpacesTitle";
import SideBar from "../../Components/sideBar/SideBar";
import "./styles.scss";
import React, { useEffect, useState } from "react";
import { ACCESS_TOKEN } from "../../Contains/Config";
import Menu from "../HomePage/components/Menu";
import { useNavigate } from "react-router-dom";
import { apiClient } from "../../Services";
import {
  Button,
  Progress,
  Select,
  Table,
  Tag,
  Tooltip,
  notification,
  message,
  Spin,
} from "antd";
import GroundAvatar from "../../Components/common/GroundAvatar";
import { LoadingOutlined, MailOutlined } from "@ant-design/icons";
import moment from "moment";

export default function ProjectManagement() {
  const [member, setMember] = useState([]);
  const [boardList, setBoardList] = useState([]);
  const [boardId, setBoardId] = useState(null);
  const [cardList, setCardList] = useState([]);
  const [loadingReminders, setLoadingReminders] = useState({});

  let navigate = useNavigate();
  const token = localStorage.getItem(ACCESS_TOKEN);

  useEffect(() => {
    if (token) {
      return;
    }
    navigate("/login");
  }, [token]);

  useEffect(() => {
    if (boardId) {
      try {
        apiClient.fetchApiGetMemberProjects(boardId).then((res) => {
          if (res.data) {
            setMember(res.data);
          } else {
            setMember([]);
          }
        });
      } catch (e) {
        setMember([]);
      }
    }
  }, [boardId]);

  useEffect(() => {
    if (boardId) {
      try {
        apiClient.fetchApiGetTabs(boardId).then((res) => {
          if (res.data !== null && res.data) {
            const columnsF = res.data;

            const cards = columnsF
              .reduce((acc, item) => {
                return [
                  ...acc,
                  ...item?.cards?.map((cardItem) => ({
                    ...cardItem,
                    totalPercent: totalPercentTask(cardItem?.tasks) || 0,
                    isComplete: totalPercentTask(cardItem?.tasks) === 100,
                  })),
                ];
              }, [])
              .sort(
                (a, b) => -1 * (new Date(a.createdOn) - new Date(b.createdOn))
              );

            setCardList(cards);
          } else {
            setCardList([]);
          }
        });
      } catch (e) {
        setCardList([]);
      }
    }
  }, [boardId]);

  useEffect(() => {
    apiClient
      .fetchApiGetProjectPerson()
      .then((res) => {
        if (res.data) {
          setBoardList(res.data);
        } else {
          setBoardList([]);
        }
      })
      .catch((e) => {
        setBoardList([]);
      });
  }, []);

  useEffect(() => {
    if (boardList?.length > 0) {
      setBoardId(boardList?.[0]?.id);
    }
  }, [boardList]);

  const totalPercentTask = (tasks) => {
    const taskCheckedCount = tasks.reduce(
      (acc, task) => (acc += Number(task.isActive)),
      0
    );

    return Math.round(Math.ceil((taskCheckedCount / tasks.length) * 100));
  };

  const columns = [
    {
      title: "Card Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Assignment",
      dataIndex: "assignment",
      key: "assignment",
      render: (text, record) => {
        return (
          <div className="avatar-group-cell">
            <GroundAvatar
              member={record?.cardUserMembers?.reduceRight((acc, item) => {
                return [...acc, item.memberNavigation];
              }, [])}
              size="default"
            />
          </div>
        );
      },
    },
    {
      title: "Deadline",
      dataIndex: "deadline",
      key: "deadline",
      width: "120px",
      render: (text, record) => {
        return moment(record?.timeExpiry).format("DD/MM/YYYY");
      },
    },
    {
      title: "Current Progress",
      dataIndex: "progress",
      key: "progress",
      render: (text, record) => {
        return <Progress percent={record?.totalPercent} />;
      },
    },
    {
      title: "Type",
      dataIndex: "type",
      key: "type",
      width: "120px",
      render: (text, record) => {
        const isComplete = record?.isComplete;

        return (
          <Tag color={isComplete ? "green" : "red"}>
            {isComplete ? "Complete" : "Incomplete"}
          </Tag>
        );
      },
    },
    {
      title: "Action",
      dataIndex: "action",
      key: "action",
      width: "80px",
      align: "center",
      render: (text, record) => {
        if (record?.isComplete || record?.cardUserMembers?.length === 0) {
          return null;
        }

        return (
          <Tooltip title="Remind">
            <Button
              type="link"
              onClick={() => handleRemind(record)}
              // loading={loadingReminders[record.id]}
              disabled={loadingReminders[record.id]}
            >
              {loadingReminders[record.id] ? (
                <Spin indicator={<LoadingOutlined spin />} />
              ) : (
                <MailOutlined style={{ fontSize: "20px" }} />
              )}
            </Button>
          </Tooltip>
        );
      },
    },
  ];

  const handleRemind = (record) => {
    if (!record || !record.id) {
      return;
    }

    // Set loading state for this specific card
    setLoadingReminders((prev) => ({
      ...prev,
      [record.id]: true,
    }));

    // Show loading message
    const loadingMessage = message.loading("Sending reminder emails...", 0);

    apiClient
      .fetchApiRemindCardMembers(record.id)
      .then((res) => {
        // Close the loading message
        loadingMessage();

        // Reset loading state
        setLoadingReminders((prev) => ({
          ...prev,
          [record.id]: false,
        }));

        // Check if some emails failed
        if (
          res.data &&
          typeof res.data === "string" &&
          res.data.includes("Some emails failed")
        ) {
          notification.warning({
            message: "Partial Success",
            description: res.data,
            placement: "topRight",
            duration: 5,
          });
        } else {
          notification.success({
            message: "Success",
            description: "Reminder emails sent successfully!",
            placement: "topRight",
            duration: 4,
          });
        }
      })
      .catch((err) => {
        // Close the loading message
        loadingMessage();

        // Reset loading state
        setLoadingReminders((prev) => ({
          ...prev,
          [record.id]: false,
        }));

        console.error("Error sending reminders:", err);

        // Show more detailed error message if available
        let errorMessage =
          "Failed to send reminder emails. Please try again later.";
        if (err.response && err.response.data) {
          if (typeof err.response.data === "string") {
            errorMessage = err.response.data;
          } else if (err.response.data.message) {
            errorMessage = err.response.data.message;
          }
        }

        notification.error({
          message: "Error",
          description: errorMessage,
          placement: "topRight",
          duration: 6,
        });
      });
  };

  return (
    <>
      <Menu />
      {boardList?.length === 0 && (
        <Container className="main-content-container">
          <Row className="main-row">
            <div
              style={{
                fontWeight: "700",
                fontSize: "26px",
                textAlign: "center",
                marginBottom: "12px",
              }}
            >
              No project found
            </div>
          </Row>
        </Container>
      )}
      {boardList?.length > 0 && (
        <Container className="main-content-container">
          <Row className="main-row">
            <div
              style={{
                fontWeight: "700",
                fontSize: "26px",
                textAlign: "center",
                marginBottom: "12px",
              }}
            >
              Project management
            </div>
            <div
              style={{
                marginBottom: "8px",
              }}
            >
              Project:{" "}
              <Select
                style={{ minWidth: "200px" }}
                value={boardId}
                showSearch
                optionFilterProp="label"
                onChange={(value) => setBoardId(value)}
                options={boardList?.map((item) => ({
                  label: item.name,
                  value: item.id,
                }))}
              />
            </div>
            <div
              style={{
                marginBottom: "8px",
                display: "flex",
                alignItems: "center",
                gap: "8px",
              }}
            >
              Member: <GroundAvatar member={member} />
            </div>
            <Table
              size="middle"
              rowKey="id"
              columns={columns}
              dataSource={cardList}
              pagination={{
                // pageSize: pagination.pageSize,
                // current: pagination.page,
                // total: pagination.total,
                showTotal: (total, range) => `${total} items`,
              }}
            />
          </Row>
        </Container>
      )}
    </>
  );
}
