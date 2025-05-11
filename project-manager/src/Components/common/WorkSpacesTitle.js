import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import { Link } from "react-router-dom";
import "./WorkSpacesTitle.scss";

function WorkSpacesTitle({ title, iconName }) {
  return (
    <div className="work-space-title">
      <h3>{title}</h3>
      <Link to="">
        <FontAwesomeIcon className="icon" icon={iconName} />
      </Link>
    </div>
  );
}

export default WorkSpacesTitle;
