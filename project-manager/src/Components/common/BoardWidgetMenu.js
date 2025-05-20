import Modal from "antd/lib/modal/Modal";
import React, { useState } from "react";
import { Input, Select } from "antd";
import { BACKGROUD_BOARD } from "../../actions/dataBackgroud";
import { useNavigate } from "react-router-dom";
import "./BoardWidgetMenu.scss";
function BoardWidgetMenu({ largeWidget, avt, title, id }) {
  const [background, setBackground] = useState(BACKGROUD_BOARD[0]);
  const [indexBg, setIndexBg] = useState(0);

  const navigate = useNavigate();
  const handleClick = () => {
    window.location.href = `/board/${id}/schedule`;
  };

  return (
    <div
      className={`board-widget ${largeWidget ? "large" : ""}`}
      onClick={handleClick}
    >
      <div className="board-thumbnail">
        <img src={avt} alt={title} />
      </div>
      <div className="board-info">
        <span className="board-title">{title}</span>
      </div>
    </div>
  );
}

export default BoardWidgetMenu;
