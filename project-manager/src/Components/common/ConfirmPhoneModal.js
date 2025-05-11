import React from "react";
import "antd/dist/antd.css";
import { Modal, Button, Input } from "antd";
function ConfirmPhoneModal(props) {
  const {
    forgotPassword,
    handleSendCode,
    handleGetCode,
    handleChangeCode,
    isModalVisible,
    handleOk,
    handleCancel,
    handleChangeRepeatEmail
  } = props;

  return (
    <div>
      <Modal
        title="Xác thực tài khoản"
        visible={isModalVisible}
        onOk={handleOk}
        onCancel={handleCancel}
      >
        {forgotPassword && (
          <Input
          style={{marginTop: '10px'}}
          placeholder="Vui lòng nhập lại email"
          onChange={handleChangeRepeatEmail}
          />
          )}
          <Input
            style={{marginTop: '10px'}}
            placeholder="Nhập mã code đã được gửi vào số điện thoại"
            onChange={handleChangeCode}
          />
        {forgotPassword || <Button onClick={handleGetCode}>Lấy mã</Button>}

        <Button  style={{marginTop: '10px'}} onClick={handleSendCode}>Gửi mã</Button>
      </Modal>
    </div>
  );
}

export default ConfirmPhoneModal;
