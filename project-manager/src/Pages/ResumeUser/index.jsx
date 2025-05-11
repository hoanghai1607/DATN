import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import avatar from "./../../Assets/images/user4.png";
import { BASE_URL_IMAGE } from "../../Contains/ConfigURL";
import Footer from "../Footer";
import Menu from "../HomePage/components/Menu";
import { apiClient } from "../../Services";

const ResumeUser = (props) => {
    const {resumeUser} = props;
    const [dataUser, setDataUser] = useState({});
    const {userID} = useParams();
    useEffect(() => {
        apiClient
			.fetchApiGetUserID(userID)
			.then((res) => {
				if (res.data) {
					setDataUser(res.data);
				} else {
					// throw Exception("Không có tài khoản.")
					console.log("You dont account.");
				}
			})
			.catch((e) => {
				// console.log(e);
			});
    }, [])
    return (
        <>
            <Menu />
            <div className="main-setting">
				<div className="main-content">
					<div className="avatar-box">
						<img
							src={
								dataUser.urlAvatar
									? BASE_URL_IMAGE + "/" + dataUser.urlAvatar
									: avatar
							}
							alt="avatar"
						/>
					</div>
					<div className="name-box">
						<h5 className="avatar-name">
							{dataUser.firstname + dataUser.lastname}
						</h5>
					</div>
					<div className="user-name">
						<p>@{dataUser.email}</p>
					</div>
                    <div className="user-name">
						<p>Contact me: {dataUser.phoneNumber}</p>
					</div>
                    
				</div>
            </div>
            <Footer />
        </>
    )
}

export default ResumeUser;