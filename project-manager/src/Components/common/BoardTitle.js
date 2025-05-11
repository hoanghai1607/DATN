import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "./BoardTitle.scss";

function BoardTitle({title, iconName}) {
  return (
        <div className="title-content">
          <span>
           <FontAwesomeIcon className="icon" icon={iconName} />
          </span>
          <h3>{title}</h3>
        </div>
  );
}

export default BoardTitle;
