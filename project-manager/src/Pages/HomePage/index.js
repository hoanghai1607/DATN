import { Col, Container, Row } from "react-bootstrap";
import BoardCard from "../../Components/common/BoardCard";
import BoardTitle from "../../Components/common/BoardTitle";
import CreateBoard from "../../Components/common/CreateBoard";
import WorkSpacesTitle from "../../Components/common/WorkSpacesTitle";
import SideBar from "../../Components/sideBar/SideBar";
import "./styles.scss";
import React, { useEffect, useState } from "react";
import { ACCESS_TOKEN } from "../../Contains/Config";
import Menu from "./components/Menu";
import { useNavigate } from 'react-router-dom';
import { apiClient } from "../../Services";

export default function HomePage() {
	const [boardPerson, setBoardPerson] = useState([]);
	const [projectPerson, setProjectPerson] = useState([]);

	let navigate = useNavigate();
	const token = localStorage.getItem(ACCESS_TOKEN);
	useEffect(() => {
		if (token) {
			return;
		}
		navigate("/login");
	}, [token]);

	useEffect(() => {
		apiClient.fetchApiGetProjects().then(res => {
			if (res.data) {
				setBoardPerson(res.data)
			} else {
			}

		}).catch(e => {
		});
	}, [])

	useEffect(() => {
		apiClient.fetchApiGetProjectPerson().then(res => {
			if (res.data) {
				setProjectPerson(res.data);
			} else {
				// console.log("Fail....")
			}

		}).catch(e => {
			// console.log(e);
		});
	}, [])

	return (
		<>
			<Menu />
			<Container className="main-content-container">
				<Row className="main-row">
					<Col className="sub-menu" sm={3}>
						<SideBar projectPerson={projectPerson}/>
					</Col>
					<Col className="main-content" sm={9}>
						<Row>
							<Col sm={12} >
								<BoardTitle
									title="Recently viewed"
									iconName="fa-solid fa-clock"
								/>
								<BoardCard boardPerson={boardPerson}/>
							</Col>
						</Row>
						<Row>
							<Col sm={12}>
								<WorkSpacesTitle title="YOUR WORKSPACES" />
								<BoardTitle
									title="Projects"
									iconName="fa-solid fa-clock"
								/>
								<BoardCard boardPerson={boardPerson}/>
								<CreateBoard />
							</Col>
						</Row>
						<Row>
							<Col sm={12}>
								<WorkSpacesTitle iconName="fa-solid fa-circle-info" title="GUEST WORKSPACES" />
								{/* <BoardTitle
									title="Đồ Án Web"
									iconName="fa-solid fa-clock"
								/> */}
								{/* <BoardCard /> */}
							</Col>
						</Row>
					</Col>
				</Row>
			</Container>
		</>
	);
}
