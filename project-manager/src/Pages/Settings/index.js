import Menu from "../HomePage/components/Menu";
import avatar from "./../../Assets/images/user4.png";
import banner from "./../../Assets/images/baner.svg";
import { Form, Tabs, Input, Upload, Button, Radio, DatePicker } from "antd";
import ConfirmPhoneModal from "../../Components/common/ConfirmPhoneModal";
import { UploadOutlined } from "@ant-design/icons";
import "./style.scss";
import { useState, useEffect } from "react";
import { apiClient } from "../../Services";
import { BASE_URL_IMAGE } from "../../Contains/ConfigURL";
import ImgCrop from "antd-img-crop";
import { alertErrors, alertSuccess } from "../../Contains/Config";
import Footer from "../Footer";
import moment from "moment";

const { TabPane } = Tabs;
const Settings = () => {
	const [form] = Form.useForm();
	let [validate, setValidate] = useState("");
	// update infor user
	const [firstName, setFirstname] = useState("");
	const [lastName, setLastName] = useState("");
	const [address, setAddress] = useState("");
	const [gender, setGender] = useState('');
	const [dateOfBirth, setDateOfBirth] = useState("");

	let [loading, setLoading] = useState(false);
	const [dataUser, setDataUser] = useState({});
	const [fileList, setFileList] = useState([]);
	// confirm phoneNumber
	const [isModalVisible, setIsModalVisible] = useState(false);
	const [code, setCode] = useState("");

	useEffect(() => {
		apiClient
			.fetchApiUser()
			.then((res) => {
				if (res.data) {
					setDataUser(res.data);
					setDateOfBirth(res.data.dateOfBirth)
					setGender(res.data.gender)
				} else {
					// throw Exception("Không có tài khoản.")
					// console.log("You dont account.");
				}
			})
			.catch((e) => {
				// console.log(e);
			});
	}, []);
	// update info user
	const handleSave = () => {
		const data = {
			firstname: firstName,
			lastname: lastName,
			address: address,
			gender: gender,
			dateOfBirth: dateOfBirth,
		};
		apiClient
			.fetchApiUpdateUser(data)
			.then((res) => {
				alertSuccess("Chỉnh sửa thông tin thành công!", 2000);
				window.location.reload("/settings");

			})
			.catch((e) => {
				alertErrors("Đã xãy ra lỗi vui lòng thử lại");
			});
	};
	// confirm phoneNumber

	const showModal = () => {
		setIsModalVisible(true);
	};
	const handleGetCode = () => {
		apiClient
			.fetchApiUserAccountVerification()
			.then((res) => {
				alertSuccess("Hệ thống đã gửi mã về số điện thoại.", 2000);
				setLoading(false);
			})
			.catch((e) => {
				setLoading(false);
				alertErrors("Đã xãy ra lỗi vui lòng thử lại");
			});
	};

	const handleSendCode = () => {
		let data = {
			code: code,
		};
		apiClient
			.fetchApiUserAccountVerificationPost(data)
			.then((res) => {
				alertSuccess("Xác thực thành công.", 2000);
				setLoading(false);
				window.location.reload("/settings");
			})
			.catch((e) => {
				setLoading(false);
				alertErrors("Đã xãy ra lỗi vui lòng thử lại");
			});
	};
	const handleOk = () => {
		setIsModalVisible(false);
	};

	const handleCancel = () => {
		setIsModalVisible(false);
	};

	const handleChangeCode = (e) => {
		setCode(e.target.value);
	};

	const handleSubmit = (values) => {
		setLoading(true);
		let data = {
			currentPassword: values.currentPassword,
			newPassword: values.newPassword,
			checkPassword: values.checkPassword,
		};

		apiClient
			.fetchApiChangePassword(data)
			.then((res) => {
				if (res.data) {
					alertSuccess("Change Password for Success.", 2000);
					setLoading(false);
					form.resetFields();
				} else {
					setLoading(false);
					alertErrors("Change Password FOR Fail.", 2000);
				}
			})
			.catch((e) => {
				setLoading(false);
				alertErrors("Change Password FOR Fail.");
			});
		form.resetFields();
	};

	const onChange = (file) => {
		// setFileList(newFileList);
		const formData = new FormData();
		formData.append("file", file);
		
		return apiClient.fetchApiUpdateImage(formData).then((res) => {
			if (res.data) {
				window.location.reload("/settings");
				alertSuccess("Cập nhật thành công.");
			} else {
				// throw Exception("Không có tài khoản.")
				alertErrors("Vui lòng thử lại.");
			}
		});
	};

	const onPreview = async (file) => {
		let src = file.url;
		const image = new Image();
		image.src = src;
	};

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
				</div>
				<div className="main-body">
					<Tabs defaultActiveKey="1">
						<TabPane tab="Thông tin và Hồ sơ" key="1">
							<img src={banner} alt="banner" />
							<h4>Quản lý thông tin cá nhân của bạn</h4>
							<div className="description">
								<p>
									Đây là tài khoản Atlassian. Chỉnh sửa thông tin cá nhân và cài
									đặt hiển thị của bạn thông qua{" "}
									<a style={{ color: "#026dc4" }}>Hồ sơ Atlassian.</a>
								</p>
								<p>
									Để tìm hiểu thêm, vui lòng xem{" "}
									<a style={{ color: "#026dc4" }}>Điều khoản dịch vụ</a> hoặc{" "}
									<a style={{ color: "#026dc4" }}>Chính sách Riêng tư.</a>
								</p>
							</div>
							<hr
								style={{
									height: 1,
								}}
							/>
							<div className="main-input">
								<div className="name-input">
									<p
										style={{
											marginTop: "3%",
											fontSize: 15,
											fontWeight: "bold",
										}}
									>
										Họ và tên đệm
									</p>
									<input
										className="input-name"
										placeholder="Nhập họ và tên đệm"
										defaultValue={dataUser.firstname}
										onChange={(e) => setFirstname(e.target.value)}
									/>
								</div>
								<div className="name-input">
									<p
										style={{
											marginTop: "3%",
											fontSize: 15,
											fontWeight: "bold",
										}}
									>
										Tên
									</p>
									<input
										className="input-name"
										placeholder="Nhập tên"
										defaultValue={dataUser.lastname}
										onChange={(e) => setLastName(e.target.value)}
									/>
								</div>
								<div className="name-input">
									<p
										style={{
											marginTop: "3%",
											fontSize: 15,
											fontWeight: "bold",
										}}
									>
										Địa chỉ
									</p>
									<input
										className="input-name"
										placeholder="Nhập địa chỉ"
										defaultValue={dataUser.address}
										onChange={(e) => setAddress(e.target.value)}
									/>
								</div>

								<div className="name-input">
									<p
										style={{
											marginTop: "3%",
											fontSize: 15,
											fontWeight: "bold",
										}}
									>
										Giới tính
									</p>
									<Radio.Group
										value={gender}
										onChange={(e) => setGender(e.target.value)}
									>
										<Radio value={false}>Female</Radio>
										<Radio value={true}>Male</Radio>
									</Radio.Group>
								</div>
								<div className="name-input">
									<p
										style={{
											marginTop: "3%",
											fontSize: 15,
											fontWeight: "bold",
										}}
									>
										Ngày sinh
									</p>
									<DatePicker
										value={moment(dateOfBirth)}
										onChange={(date) =>
											setDateOfBirth(
												date.locale("hn").format("YYYY-MM-DD[T]HH:mm:ss.SSS[Z]")
											)
										}
									/>
								</div>
								<div className="confirm-input">
									<p
										style={{
											marginTop: "3%",
											fontSize: 15,
											fontWeight: "bold",
										}}
									>
										Số điện thoại
									</p>
									
									<Input
										className="input-name input-confirm"
										placeholder="Số điện thoại"
										value={dataUser.phoneNumber}
									/>
									
								</div>
								<Button onClick={handleSave} className="submit-button">
									Lưu
								</Button>
							</div>
						</TabPane>
						<TabPane tab="Hiển thị" key="2">
							<div className="main-avatar">
								<p
									style={{ marginTop: "1%", fontSize: 15, fontWeight: "bold" }}
								>
									Ảnh đại diện
								</p>
								<div className="show-avatar">
									<img
										src={
											dataUser.urlAvatar
												? BASE_URL_IMAGE + "/" + dataUser.urlAvatar
												: avatar
										}
										alt="avatar"
									/>
								</div>
								<p
									style={{
										marginTop: "-3%",
										marginBottom: "2px",
										fontSize: 15,
										fontWeight: "bold",
									}}
								>
									Thay đổi ảnh đại diện
								</p>
								<ImgCrop rotate>
									<Upload
										listType="picture-card"
										beforeUpload={onChange}
										onPreview={onPreview}
									>
										{fileList.length < 1 && "+ Upload"}
									</Upload>
								</ImgCrop>
							</div>
						</TabPane>
						<TabPane tab="Cài đặt tài khoản" key="3">
							<div className="account-setting">
								<div className="description">
									<p>
										Đây là tài khoản Atlassian. Chỉnh sửa thông tin cá nhân và
										cài đặt hiển thị của bạn thông qua{" "}
										<a style={{ color: "#026dc4" }}>Hồ sơ Atlassian.</a>
									</p>
									<p>
										Để tìm hiểu thêm, vui lòng xem{" "}
										<a style={{ color: "#026dc4" }}>Điều khoản dịch vụ</a> hoặc{" "}
										<a style={{ color: "#026dc4" }}>Chính sách Riêng tư.</a>
									</p>
								</div>
								<hr style={{ height: 1 }} />
								<h4>Chi tiết tài khoản</h4>
								<hr
									style={{
										height: 1,
									}}
								/>
								
								<hr style={{ height: 1 }} />
								<div className="main-pass-input">
									<h5>Thay đổi mật khẩu</h5>
									<Form
										name="basic"
										labelCol={{
											span: 24,
										}}
										wrapperCol={{
											span: 24,
										}}
										initialValues={{
											remember: false,
										}}
										form={form}
										onFinish={handleSubmit}
									>
										<Form.Item
											label="Current Password:"
											name="currentPassword"
											rules={[
												{
													required: true,
													message: "Please input your password!",
												},
												{
													min: 6,
													message: "Password less than 6 characters ",
												},
											]}
											style={{ marginBottom: "20px" }}
										>
											<Input.Password placeholder="Enter password" />
										</Form.Item>

										<Form.Item
											label="New Password"
											name="newPassword"
											dependencies={["currentPassword"]}
											hasFeedback
											rules={[
												{
													required: true,
													message: "Please input your password!",
												},
												{
													min: 6,
													message: "Password less than 6 characters ",
												},
												({ getFieldValue }) => ({
													validator(_, value) {
														if (
															!value ||
															getFieldValue("currentPassword") !== value
														) {
															return Promise.resolve();
														}
														return Promise.reject(
															new Error(
																"Password not same with Current Password"
															)
														);
													},
												}),
											]}
											style={{ marginBottom: "20px" }}
										>
											<Input.Password placeholder="Enter password" />
										</Form.Item>

										<Form.Item
											label="Confirm Password"
											name="checkPassword"
											dependencies={["newPassword"]}
											hasFeedback
											rules={[
												{
													required: true,
													message: "Please confirm your password!",
												},
												({ getFieldValue }) => ({
													validator(_, value) {
														if (
															!value ||
															getFieldValue("newPassword") === value
														) {
															return Promise.resolve();
														}
														return Promise.reject(
															new Error(
																"The two passwords that you entered do not match!"
															)
														);
													},
												}),
											]}
											style={{ marginBottom: "20px" }}
										>
											<Input.Password placeholder="Comfirm with password" />
										</Form.Item>

										<Form.Item
											wrapperCol={{
												offset: 0,
												span: 24,
											}}
										>
											<Button
												type="primary"
												htmlType="submit"
												style={{ width: "100%", padding: "5px 0" }}
												loading={loading}
											>
												Update Password
											</Button>
										</Form.Item>
									</Form>
								</div>
							</div>
						</TabPane>
					</Tabs>
				</div>
				<Footer />
			</div>
		</>
	);
};

export default Settings;
