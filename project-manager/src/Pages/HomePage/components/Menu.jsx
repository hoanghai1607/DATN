import React, { useEffect, useState, useRef } from "react";
import "./Menu.scss";
import {
  Html5Outlined,
  DownOutlined,
  UserAddOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import SearchComponent from "./Search";
import Settings from "./Settings";
import { Link, useNavigate } from "react-router-dom";
import { apiClient } from "../../../Services";
import AddPeopleModel from "../../../Components/common/AddPeopleModal";
import {
  ACCESS_TOKEN,
  alertErrors,
  alertSuccess,
} from "../../../Contains/Config";
import GroundAvatar from "../../../Components/common/GroundAvatar";
import { BACKGROUD_BOARD } from "../../../actions/dataBackgroud";
import CreateBoardModal from "../../../Components/common/CreateBoardModal";
import BoardWidgetMenu from "../../../Components/common/BoardWidgetMenu";

export default function Menu(props) {
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [member, setMember] = useState([]);
  const [dataUser, setDataUser] = useState([]);
  const [phoneNumber, setId] = useState("");
  const [nameText, setName] = useState("");
  const [id, checkId] = useState("");
  const [loading, setLoading] = useState(false);
  const [dataSearch, setDataSearch] = useState([]);
  const [isSearch, setIsSearch] = useState(false);
  const [isModalVisibleCreate, setIsModalVisibleCreate] = useState(false);
  const [title, setTitle] = useState("");
  const [number, setNumber] = useState("");
  const [indexBG, setIndexBG] = useState(0);
  const [g63Value, setG63Value] = useState("Hãy tìm kiếm theo cách của bạn.");

  // nam start
  const [projectPerson, setProjectPerson] = useState([]);
  // Add this near your other useState declarations
  const [showWorkspaces, setShowWorkspaces] = useState(false);
    const [showrecent, setShowRecents] = useState(false);
  const workspaceRef = useRef(null);
  const recentRef = useRef(null);
  // end

  const navigator = useNavigate();

  useEffect(() => {
    apiClient
      .fetchApiUser()
      .then((res) => {
        if (res.data) {
          setDataUser(res.data);
        } else {
          console.log("You dont account.");
        }
      })
      .catch((e) => {
        localStorage.removeItem(ACCESS_TOKEN);
        window.location.reload("/login");
      });
  }, []);

  useEffect(() => {
    if (props.boardId) {
      apiClient.fetchApiGetMemberProjects(props.boardId).then((res) => {
        if (res.data) {
          setMember(res.data);
        } else {
          // console.log("Fail.....");
        }
      });
    }
  }, []);

  useEffect(() => {
    apiClient
      .fetchApiGetProjectPerson()
      .then((res) => {
        if (res.data) {
          setProjectPerson(res.data);
          console.log("projectPerson", res.data);
        } else {
          // console.log("Fail....")
        }
      })
      .catch((e) => {
        // console.log(e);
      });
  }, []);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (
        workspaceRef.current &&
        !workspaceRef.current.contains(event.target)
      ) {
        setShowWorkspaces(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

    useEffect(() => {
    const handleClickOutside = (event) => {
      if (
        recentRef.current &&
        !recentRef.current.contains(event.target)
      ) {
        setShowRecents(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const showModal = () => {
    setIsModalVisible(true);
  };
  const handleOk = () => {
    if (id !== "") {
      let data = {
        idUser: id,
        isActive: true,
      };
      if (props.boardId) {
        apiClient
          .fetchApiPostUserProject(props.boardId, data)
          .then((res) => {
            if (res.data) {
              alertSuccess("Thêm thành công.");
              setMember([...member, nameText]);
              handleCancel();
            }
          })
          .catch((e) => {
            alertErrors("Đã tồn tại.");
            handleCancel();
          });
      }
    } else {
      setIsModalVisible(false);
    }
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    setName("");
  };
  const handleChange = (e) => {
    setId(e.target.value);
    let memberPhone = e.target.value;
    if (
      memberPhone !== "" &&
      memberPhone &&
      memberPhone.length > 9 &&
      memberPhone.length < 11
    ) {
      setLoading(true);
      setTimeout(() => {
        apiClient
          .fetchApiFindUser(memberPhone)
          .then((res) => {
            setName(res.data);
            checkId(res.data.id);
            setLoading(false);
          })
          .catch((e) => {
            setName(null);
            setLoading(false);
          });
      }, 2000);
    }
  };

  const handleFocus = () => {
    setIsSearch(true);
  };
  const onSearch = (value) => {
    if (value == "") {
      return;
    }
    console.log(value);
    apiClient.fetchApiGetNameBoard(value).then((res) => {
      if (res.data && res.data?.length > 0) {
        setDataSearch(res.data);
        setG63Value("Not Found.");
      }
    });
  };

  const onChangePageBoard = (id) => {
    window.location.href = `/board/${id}/schedule`;
  };

  const showModalCreate = () => {
    setIsModalVisibleCreate(true);
  };

  const handleOkCreate = () => {
    setIsModalVisibleCreate(true);
    let data = {
      name: title,
      numberMember: number,
      background: indexBG,
      timeExpiry: "2022-12-12T15:48:44.907Z",
    };

    apiClient
      .fetApiCreateProject(data)
      .then((res) => {
        if (res.data) {
          alertSuccess("Create Board for Success.", 2000);
          window.location.reload("/");
        } else {
          alertErrors("Create Board for Fail.", 2000);
        }
      })
      .catch((e) => {
        alertErrors("Create Board for Fail.");
      });
  };

  const handleCancelCreate = () => {
    setIsModalVisibleCreate(false);
  };
  const handleChangeTitle = (e) => {
    setTitle(e.target.value);
  };
  const handleChangeNumber = (e) => {
    setNumber(e.target.value);
  };

  const handleChangeBackGround = (index) => {
    setIndexBG(index);
  };

  return (
    <div className="main-menu">
      <div className="header__menu">
        <div className="header__menu-left">
          <div className="menu-left__logo">
            <Link to="/">
              <span className="logo">Trello</span>
            </Link>
          </div>
          <div className="menu-left__list">
            <ul className="list-dropdown">
              <li className="list-item" ref={workspaceRef}>
                <div
                  className="workspace-trigger"
                  onClick={() => setShowWorkspaces(!showWorkspaces)}
                  style={{ cursor: "pointer" }}
                >
                  WorkSpaces <DownOutlined />
                </div>
                {showWorkspaces && (
                  <div className="work-space-container">
                    {projectPerson?.length > 0 ? (
                      projectPerson.map((i, _i) => (
                        <BoardWidgetMenu
                          key={i.id}
                          id={i.id}
                          title={i.name}
                          avt={BACKGROUD_BOARD[i.background]}
                          largeWidget={true}
                        />
                      ))
                    ) : (
                      <div className="no-workspaces">
                        No workspaces available
                      </div>
                    )}
                  </div>
                )}
              </li>
              <li className="list-item"  ref={recentRef}>
                <div
                  className="workspace-trigger"
                  onClick={() => setShowRecents(!showrecent)}
                  style={{ cursor: "pointer" }}
                >
                  Recent <DownOutlined />
                </div>
                {showrecent && (
                  <div className="work-space-container">
                    {projectPerson?.length > 0 ? (
                      projectPerson.map((i, _i) => (
                        <BoardWidgetMenu
                          key={i.id}
                          id={i.id}
                          title={i.name}
                          avt={BACKGROUD_BOARD[i.background]}
                          largeWidget={true}
                        />
                      ))
                    ) : (
                      <div className="no-workspaces">
                        No workspaces available
                      </div>
                    )}
                  </div>
                )}
              </li>
              <li
                className="list-item"
                onClick={() => navigator("/project-management")}
                style={{ whiteSpace: "nowrap" }}
              >
                Project Management
              </li>
              <li className="list-item">
                <button className="btn-create" onClick={showModalCreate}>
                  Create
                </button>
                <CreateBoardModal
                  isModalVisible={isModalVisibleCreate}
                  onOk={handleOkCreate}
                  handleOk={handleOkCreate}
                  handleCancel={handleCancelCreate}
                  handleChangeNumber={handleChangeNumber}
                  handleChangeTitle={handleChangeTitle}
                  handleChangeBackGround={handleChangeBackGround}
                />
              </li>

              {props.checked == "true" ? (
                <li className="list-item">
                  <div className="gr_avatar">
                    <GroundAvatar member={member} />
                  </div>
                </li>
              ) : (
                ""
              )}

              {props.checked == "true" ? (
                <li className="list-item">
                  <button className="btn-add-create" onClick={showModal}>
                    <UserAddOutlined /> Add People
                  </button>
                  <AddPeopleModel
                    isModalVisible={isModalVisible}
                    handleOk={handleOk}
                    handleCancel={handleCancel}
                    handleChange={handleChange}
                    nameText={nameText}
                    id={id}
                    loading={loading}
                  />
                </li>
              ) : (
                ""
              )}
            </ul>
          </div>
        </div>
        <div className="header__menu-right">
          <SearchComponent onSearch={onSearch} onFocus={handleFocus} />
          {isSearch && (
            <div className="list__item-search">
              <h6 className="list-title">Board</h6>
              <div className="close__search" onClick={() => setIsSearch(false)}>
                <CloseOutlined />
              </div>
              <div className="data-search">
                <ul>
                  {dataSearch.length > 0 &&
                    dataSearch.map((i, _i) => (
                      <li
                        className="board__item"
                        key={_i}
                        onClick={() => onChangePageBoard(i.id)}
                      >
                        <div className="img-board">
                          <img src={BACKGROUD_BOARD[i.background]} />
                        </div>
                        <p className="board-name-c003">{i.name}</p>
                      </li>
                    ))}
                  {dataSearch.length <= 0 && (
                    <p style={{ textAlign: "center" }}>{g63Value}</p>
                  )}
                </ul>
              </div>
            </div>
          )}
          <Settings dataUser={dataUser} />
        </div>
      </div>
    </div>
  );
}
