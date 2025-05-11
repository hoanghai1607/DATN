import { Col, Container, Row } from "react-bootstrap";

import SideBar from "../../Components/sideBar/SideBar";
import "./styles.scss";
import React, { useEffect } from "react";
import { ACCESS_TOKEN } from "../../Contains/Config";
import { useNavigate } from "react-router-dom";
import Menu from "../HomePage/components/Menu";
import "antd/dist/antd.css";
import { Card } from "antd";
import BoardWidget from "../../Components/common/BoardWidget";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function WorkSpaces() {
	const { Meta } = Card;
	let navigate = useNavigate();
	const token = localStorage.getItem(ACCESS_TOKEN);
	useEffect(() => {
		if (token) {
			return;
		}
		navigate("/login");
	}, [token]);

	return (
		<>
			<Menu />
			<Container className="main-content-container">
				<Row className="main-row">
					<Col sm={3}>
						<SideBar />
					</Col>
					<Col className="work-space-main-content" sm={6}>
						<div className="card-wrapper">
							<p>When you’re added to a checklist item, it’ll show up here.</p>
							<Card
								hoverable
								className="card-content"
								cover={
									<img
										alt="example"
										src="https://a.trellocdn.com/prgb/dist/images/home/orientation/no-content.e55b3540e5c1f06a51d7.svg"
									/>
								}
							>
								<Meta
									title="Stay on track and up to date"
									description="Invite people to boards and cards, leave comments, add due dates, and we'll show the most important activity here."
								/>
							</Card>
							,
						</div>
					</Col>
					<Col className="side-content" sm={3}>
						<Row>
							<div className="side-title">
								<FontAwesomeIcon
									className="icon-trello"
									icon="fa-solid fa-clock"
								/>
								<span>Recently View</span>
							</div>
						</Row>
						<Row>
							<Col sm={12}>
								{/* <BoardWidget
									title="Zola-App"
									avt="https://i.pinimg.com/736x/76/07/5c/76075c11bfe509ee9a11d9baa991c40d.jpg"
									largeWidget={true}
								/> */}

							</Col>
						</Row>
					</Col>
				</Row>
			</Container>
		</>
	);
}
