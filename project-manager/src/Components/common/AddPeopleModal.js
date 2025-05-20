import React from "react";
import "antd/dist/antd.css";
import { Modal, Button, Input } from "antd";
import { BASE_URL_IMAGE } from "../../Contains/ConfigURL";
import Loading from "../../Pages/Loading";

function AddPeopleModel(props) {
  const {
    handleChange,
    isModalVisible,
    handleOk,
    handleCancel,
    nameText,
    loading,
  } = props;

  return (
    <div>
      <Modal
        title="Add people in Project"
        visible={isModalVisible}
        onOk={handleOk}
        onCancel={handleCancel}
        okText="Add People"
        cancelText="Check"
      >
        <Input
          name="id"
          placeholder="Nhập số điện thoại"
          onChange={handleChange}
        />
        {/* <Button onClick={handleCheck}>Check</Button> */}
        {loading ? (
          <div className="loading-add-user">
            <Loading type="spin" color="blue" />
          </div>
        ) : (
          <p>
            {nameText ? (
              <>
                <div className="list-client-37">
                  <img
                    src={
                      !nameText.urlAvatar
                        ? "https://static.vecteezy.com/system/resources/previews/009/292/244/non_2x/default-avatar-icon-of-social-media-user-vector.jpg"
                        : BASE_URL_IMAGE + "/" + nameText.urlAvatar
                    }
                  ></img>
                  <div className="list-client-37-name">
                    <h6>
                      {nameText.firstname + nameText.lastname}
                      <p>{nameText.email}</p>
                    </h6>
                  </div>
                </div>
              </>
            ) : nameText == null ? (
              "Không tìm thấy Member."
            ) : (
              ""
            )}
          </p>
        )}
      </Modal>
    </div>
  );
}

export default AddPeopleModel;
