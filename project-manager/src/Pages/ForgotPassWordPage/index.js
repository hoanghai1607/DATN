import React, { useState } from "react";
import "antd/dist/antd.css";
import "./styles.scss";
import { apiClient } from "../../Services";
import { alertErrors, alertSuccess } from "../../Contains/Config";
import { Button, Card, Input } from "antd";
import { useNavigate } from 'react-router-dom';
import Footer from "../Footer";
import ConfirmPhoneModal from "../../Components/common/ConfirmPhoneModal";
function ForgotPassWordPage() {
  const { Search } = Input;
  const [email, setEmail] = useState("");
  const [repeatEmail, setRepeatEmail] = useState("")
  const [isModalVisible, setIsModalVisible] = useState(false);
  let [loading, setLoading] = useState(false);
  let navigate = useNavigate();

  const [code, setCode] = useState("");

  const handleChangeEmail = (e) => {
    setEmail(e.target.value);
  };
  // confirm phoneNumber
  const handleChangeRepeatEmail = (e) => {
    setRepeatEmail(e.target.value);
  };
  const showModal = () => {
    // console.log(email);
    let data = {
      email: email,
    };
    apiClient
      .fetchApiForgotPassWordCode(data)
      .then((res) => {
        alertSuccess("Hệ thống đã gửi mã về số điện thoại.", 2000);
        setLoading(false);
        setIsModalVisible(true);
    })
    .catch((e) => {
        setLoading(false);
        alertErrors("Email không tìm thấy");
        setIsModalVisible(false);
    });
};
const handleSendCode = () => {
    let data = {
        code: code,
        email: email
    };
    apiClient
    .fetchApiForgotPassWord(data)
    .then((res) => {
        alertSuccess("Thay đổi mật khẩu thành công.", 2000);
        setLoading(false);
        navigate("/login")
        
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

  return (
    <div className="forgot-container">
      <div className="site-card-border-less-wrapper">
        <Card title="Find Your Account" bordered={true}>
          <p>
            Please enter your email address or mobile number to search for your
            account.
          </p>
          <Input.Group compact style={{marginTop: "50px" }}>
            <Input
              style={{ width: "calc(100% - 200px)" }}
              placeholder="Enter your email address"
              onChange={handleChangeEmail}
            />
            <Button type="primary" onClick={showModal}>Xác nhận</Button>
          </Input.Group>
          <ConfirmPhoneModal
            isModalVisible={isModalVisible}
            handleOk={handleOk}
            handleChangeCode={handleChangeCode}
            handleCancel={handleCancel}
            handleSendCode={handleSendCode}
            handleChangeRepeatEmail={handleChangeRepeatEmail}
            forgotPassword={true}
          />
        </Card>
      </div>
      <Footer />
    </div>
  );
}

export default ForgotPassWordPage;
