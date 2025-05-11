import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from "react-router-dom";
import "./BoardWidget.scss";

function BoardWidget({ largeWidget, avt, title, id}) {
	return (
		<div>
			<Link to={`/board/${id}/schedule`}>
				<div className={`content-wrapper ${largeWidget ? "large" : "small"}`}>
					{avt ? (
						<img src={avt} alt=""></img>
					) : (
						<FontAwesomeIcon
							className="icon-trello"
							icon="fa-brands fa-trello"
						/>
					)}
					<span>{title}</span>
				</div>
			</Link>
		</div>
	);
}

export default BoardWidget;
