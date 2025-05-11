import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link, useNavigate } from "react-router-dom";
import "./SideBar.scss";
import BoardWidget from "../common/BoardWidget";
import { BACKGROUD_BOARD } from "../../actions/dataBackgroud";
function SideBar(props) {
	const navigate = useNavigate();
	const { projectPerson } = props;
	const location = window.location.pathname;
	const handleClick = (key) => {
		navigate(key);
	}

	return (
		<div className="side-bar-container">
			<div className="nav-container">
				<Link to="/"
					onClick={() => handleClick("/")}
				>
					<div className={`content-wrapper ${location === "/" ? 'active' : null}`}>
						<span>
							<FontAwesomeIcon icon="fa-brands fa-trello" />
						</span>
						<span>Boards</span>
					</div>
				</Link>
				<Link to="/workspace"
					onClick={() => handleClick("/workspace")}

				>
					<div className={`content-wrapper ${location === "/workspace" ? 'active' : null}`}>
						<span>
							<FontAwesomeIcon icon="fa-solid fa-bolt" />
						</span>
						<span>Home</span>
					</div>
				</Link>
				<div className="work-space-container">
					<p>Workspaces</p>
					<Link to="" className="icon-plus">
						<FontAwesomeIcon icon="fa-solid fa-plus" />
					</Link>
				</div>
				{
					projectPerson?.length > 0 && projectPerson.map((i, _i) => (
						<BoardWidget
							key={i.id}
							id={i.id}
							title={ i.name }
							avt={BACKGROUD_BOARD[i.background]}
							largeWidget={true}
						/>
					))
				}
			</div>
		</div>
	);
}

export default SideBar;
