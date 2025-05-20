import { Input, Button, Modal, Progress, Checkbox, DatePicker } from "antd";
import TextArea from "antd/lib/input/TextArea";
import { useState, useEffect, useRef } from "react";
import { CloseOutlined, SisternodeOutlined } from "@ant-design/icons";
import "./style.scss";
import TaskCard from "../TaskCard";
import { applyDrag } from "../../../Utils/dragDrop";
import { apiClient } from "../../../Services";
import NotFound from "../../NotFound";
import { mapOrderCol } from "../../../Utils/sort";
import _ from "lodash";
import GroundAvatar from "../../../Components/common/GroundAvatar";
import { BASE_URL_IMAGE } from "../../../Contains/ConfigURL";
import moment from "moment";

const ModalCard = (props) => {
  const {
    isModalVisible,
    handleOk,
    handleCancel,
    card,
    titleCard,
    setTitleCard,
    descriptionCard,
    setDescriptionCard,
    memberCard,
    memberMoreOne,
    handleUpdateCardMember,
  } = props;
  const [isShowForm, setIsShowForm] = useState(false);
  const inputRef = useRef(null);
  const [data, setData] = useState([]);
  const [percent, setPercent] = useState(0);
  const [isShowMember, setIsShowMember] = useState(false);
  const [deadline, setDeadline] = useState(null);
  const [deadlineErrorMsg, setDeadlineErrorMsg] = useState("");

  useEffect(() => {
    apiClient
      .fetchApiGetTasks(card.id)
      .then((res) => {
        if (res.data !== null && res.data) {
          setData(mapOrderCol(res.data));
          totalPercentTask(res.data);
        } else {
          return <NotFound />;
        }
      })
      .catch((e) => {
        return <NotFound />;
      });
  }, []);

  useEffect(() => {
    if (isShowForm === true && inputRef && inputRef.current) {
      inputRef.current.focus();
    }
  }, [isShowForm]);

  useEffect(() => {
    setDeadline(moment(card.timeExpiry));
  }, [card]);

  const onTaskListDrop = (dropResult) => {
    let newData = [...data];
    newData = applyDrag(newData, dropResult);
    setData(newData);
    let dataTasks = {};
    for (let index = 0; index < newData.length; index++) {
      dataTasks = {
        name: newData[index].name,
        numberMember: 5,
        icon: "Z",
        comment: "Duy Phương",
        timeExpiry: "2022-07-07T06:14:16.538Z",
        isActive: newData[index].isActive,
        order: index,
      };
      apiClient.fetchApiUpdateTask(data[index].id, dataTasks).then((res) => {
        if (res.data) {
          // console.log("Update success....");
        } else {
          // console.log("Update Fail....");
        }
      });
    }
  };

  const handleAddTaskNew = (newData) => {
    const _tasks = _.cloneDeep(data);
    _tasks.push(newData);
    const taskData = [];
    _tasks.forEach((e) => {
      if (e !== undefined) {
        taskData.push(e);
      }
    });
    setData(taskData);
  };

  const handleRemoveTask = (index) => {
    const itemTaskId = data[index].id;
    data.splice(index, 1);
    setData([...data]);
    totalPercentTask(data);
    if (itemTaskId) {
      apiClient.fetchApiDeleteTask(itemTaskId).then((res) => {
        if (res.data) {
          // console.log("Delete.......");
        } else {
          // console.log("Delete.....f");
        }
      });
    }
  };

  const handlCardMember = (user) => {
    handleUpdateCardMember(user);
  };

  const onChange = (e, _i) => {
    data[_i].isActive = e.target.checked;
    const changeDateChecked = {
      name: data[_i].name,
      numberMember: 5,
      icon: "Z",
      comment: "Duy Phương",
      timeExpiry: "2022-07-07T06:14:16.538Z",
      isActive: data[_i].isActive,
      order: data[_i].order,
    };
    apiClient.fetchApiUpdateTask(data[_i].id, changeDateChecked).then((res) => {
      if (res.data) {
        // console.log("Update...s");
      } else {
        // console.log("Update...f");
      }
    });
    totalPercentTask(data);
  };
  const onOk = () => {
    if (!deadline) {
      setDeadlineErrorMsg("Deadline is required");
      return;
    }

    let data = {
      name: titleCard,
      description: descriptionCard,
      timeExpiry: deadline,
    };
    apiClient.fetchApiUpdateCardName(card.id, data).then((res) => {
      setTitleCard(titleCard);
      props.handleOk(titleCard);
    });
  };

  const totalPercentTask = (tasks) => {
    const taskCheckedCount = tasks.reduce(
      (acc, task) => (acc += Number(task.isActive)),
      0
    );
    setData([...tasks]);
    setPercent(Math.round(Math.ceil((taskCheckedCount / tasks.length) * 100)));
  };

  return (
    <>
      <Modal
        visible={isModalVisible}
        onOk={onOk}
        onCancel={handleCancel}
        destroyOnClose
      >
        {card.image && (
          <img
            className="card-cover"
            width="100%"
            src={card.image}
            onMouseDown={(e) => e.preventDefault()}
          />
        )}
        <input
          style={{ width: "87%" }}
          type="text"
          value={titleCard}
          onChange={(e) => setTitleCard(e.target.value)}
          className="card_value_title"
          onClick={() => setIsShowMember(false)}
        />
        <p className="sub-title">in list Current Tab</p>
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "end",
          }}
        >
          <div>
            <h6>Deadline :</h6>
            <DatePicker
              value={deadline}
              format="DD/MM/YYYY"
              onChange={(value) => {
                setDeadline(value);
                if (!value) {
                  setDeadlineErrorMsg("Deadline is required");
                } else {
                  setDeadlineErrorMsg("");
                }
              }}
            />
            {deadlineErrorMsg && (
              <p style={{ color: "red", fontSize: "14px" }}>
                {deadlineErrorMsg}
              </p>
            )}
          </div>
          <div className="btn-checklist" style={{ flex: 1 }}>
            <GroundAvatar member={memberMoreOne} />
            <button
              className="add-member-card"
              onClick={() => setIsShowMember(true)}
            >
              +
            </button>
            {isShowMember && (
              <div className="list-member">
                <div className="header-list d-flex">
                  <h6 className="header-title">Member</h6>
                  <span
                    className="close-list-member"
                    onClick={() => setIsShowMember(false)}
                  >
                    x
                  </span>
                </div>
                <div className="body-list">
                  <h6 className="body-title">Board members</h6>
                  {memberCard.length > 0 &&
                    memberCard.map((i, _i) => (
                      <div
                        className="body-list-member"
                        key={_i}
                        onClick={() => handlCardMember(i)}
                      >
                        <div className="member__item">
                          <div className="item__avatar">
                            <img
                              src={
                                !i.urlAvatar
                                  ? "https://static.vecteezy.com/system/resources/previews/009/292/244/non_2x/default-avatar-icon-of-social-media-user-vector.jpg"
                                  : BASE_URL_IMAGE + "/" + i.urlAvatar
                              }
                            />
                          </div>
                          <div className="item__name">
                            <p className="item__name-user">
                              {i.firstname + i.lastname}
                            </p>
                          </div>
                        </div>
                      </div>
                    ))}
                </div>
              </div>
            )}
          </div>
        </div>
        <hr></hr>
        <h6>Description :</h6>
        <TextArea
          value={descriptionCard}
          rows={4}
          placeholder="Add a more detailed description..."
          onChange={(e) => {
            setDescriptionCard(e.target.value);
          }}
        />
        {/* <div className="gr_biit" style={{ margin: "5px 0px 15px" }}>
          <Button type="primary">Save</Button>
        </div> */}
        <TaskCard
          onChange={onChange}
          percent={percent}
          setPercent={setPercent}
          tasks={data}
          onTaskListDrop={onTaskListDrop}
          card={card}
          handleAddTaskNew={handleAddTaskNew}
          handleRemoveTask={handleRemoveTask}
          memberCard={memberCard}
        />
      </Modal>
    </>
  );
};

export default ModalCard;
