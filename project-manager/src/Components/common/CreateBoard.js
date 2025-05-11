import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./CreateBoard.scss";
import "antd/dist/antd.css";
import CreateBoardModal from "./CreateBoardModal";
import { apiClient } from "../../Services";
import { alertErrors, alertSuccess } from "../../Contains/Config";

function CreateBoard() {
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [title, setTitle] = useState('')
  const [number, setNumber] = useState('')
  const [indexBG, setIndexBG] = useState(0)

  const showModal = () => {
    setIsModalVisible(true);
  };


  const handleOk = () => {
    setIsModalVisible(true);
    let data = {
      'name' : title,
      'numberMember' : number,
      'background' : indexBG,
      'timeExpiry': "2022-12-12T15:48:44.907Z",
    }

    apiClient.fetApiCreateProject(data).then(res => {
      if (res.data) {
        alertSuccess("Create Board for Success.", 2000)
        window.location.reload('/');

      } else {
        alertErrors("Create Board for Fail.", 2000)
      }

    }).catch(e => {
      alertErrors("Create Board for Fail.")
    });
  };

  const handleCancel = () => {
    setIsModalVisible(false);

  };
  const handleChangeTitle = (e) => {
    setTitle(e.target.value);
  } 
  const handleChangeNumber = (e) => {
    setNumber(e.target.value);
      
  }

  const handleChangeBackGround = (index) => {
    setIndexBG(index);
  }
  return (
    <div className="board-container">
      <Link to="" onClick={showModal}>
        <div className="content-board-wrapper">
          <p className="content-board-title">Create a new board</p>
        </div>
      </Link>
      <CreateBoardModal
        isModalVisible={isModalVisible}
        onOk={handleOk}
        handleOk={handleOk}
        handleCancel={handleCancel}
        handleChangeNumber={handleChangeNumber}
        handleChangeTitle={handleChangeTitle}
        handleChangeBackGround={handleChangeBackGround}
      />
    </div>
  );
}

export default CreateBoard;
