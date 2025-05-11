import React, { useEffect, useState } from "react";
import { Form, Input, Button, Checkbox, Select, Radio } from "antd";
import LeftBack from "./../../Assets/images/left.png";
import RightBack from "./../../Assets/images/right.png";
import Subito from "./../../Assets/images/subito.png";
import { GooglePlusOutlined } from "@ant-design/icons";
import "./../LoginPage/style.scss";
import { apiClient } from "../../Services";
import { ACCESS_TOKEN } from "../../Contains/Config";
import { Link, useNavigate } from "react-router-dom";
import Loading from "../Loading";

export default function RegisterPage(props) {
  const { Option } = Select;
  //   const [form] = Form.useForm();
  const navigate = useNavigate();
  let [validate, setValidate] = useState("");
  let [loading, setLoading] = useState(false);

  const handleSubmit = (values) => {
    setLoading(true);
    let data = {
      firstname: values.firstname,
      lastname: values.lastname,
      email: values.email,
      phoneNumber: values.phone,
      gender: values.gender,
      password: values.password,
    };

    apiClient
      .fetchApiSignUp(data)
      .then((res) => {
        if (res.data.success === true) {
          localStorage.setItem(ACCESS_TOKEN, JSON.stringify(res.data.token));
          navigate("/");
          window.location.reload("/");
        } else {
          setLoading(false);
          setValidate("Đăng Kí Không Thành Công");
          navigate("/sign-up");
        }
      })
      .catch((e) => {
        setLoading(false);
        setValidate("Đăng kí Không thành công");
        navigate("/sign-up");
      });
  };

  return (
    <>
      {loading && (
        <div className="loading-style">
          <Loading type="bars" color="white" />{" "}
        </div>
      )}
      <div className="main-LOGI">

        <div className="main-content">
          <div className="card-box">
            <h5 className="card-header">Sign Up to Trello</h5>
            <p style={{ textAlign: "center" }}>{validate ? validate : ""}</p>
            <div className="card-body">
              <Form
                name="basic"
                // form={form}
                labelCol={{
                  span: 24,
                }}
                wrapperCol={{
                  span: 24,
                }}
                initialValues={{
                  remember: true,
                }}
                onFinish={handleSubmit}
              >
                <Form.Item
                  // label="Email"
                  name="firstname"
                  rules={[
                    {
                      required: true,
                      message: "Please input your Firstname!",
                    },
                  ]}
                  style={{ marginBottom: "20px" }}
                >
                  <Input placeholder="Enter address Firstname" />
                </Form.Item>

                <Form.Item
                  // label="Email"
                  name="lastname"
                  rules={[
                    {
                      required: true,
                      message: "Please input your Lastname!",
                    },
                  ]}
                  style={{ marginBottom: "20px" }}
                >
                  <Input placeholder="Enter address Lastname" />
                </Form.Item>

                <Form.Item
                  // label="Email"
                  name="email"
                  rules={[
                    {
                      required: true,
                      message: "Please input your email!",
                    },
                    {
                      pattern: /^\S+@\S+\.\S+$/,
                      message: "Email wrong format!",
                    },
                  ]}
                  style={{ marginBottom: "20px" }}
                >
                  <Input placeholder="Enter address email" />
                </Form.Item>

                <Form.Item
                  name="phone"
                  rules={[
                    {
                      required: true,
                      message: "Please input your phone number!",
                    },
                  ]}
                  style={{ marginBottom: "20px" }}
                >
                  <Input placeholder="Enter your phone number" />
                </Form.Item>

                <Form.Item
                  // label="Password"
                  name="password"
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
                  name="confirm"
                  dependencies={["password"]}
                  hasFeedback
                  rules={[
                    {
                      required: true,
                      message: "Please confirm your password!",
                    },
                    ({ getFieldValue }) => ({
                      validator(_, value) {
                        if (!value || getFieldValue("password") === value) {
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
                  name="gender"
                  rules={[
                    { required: true, message: "Please pick an Gender!" },
                  ]}
                >
                  <Radio.Group>
                    <Radio.Button value={false}>Female</Radio.Button>
                    <Radio.Button value={true}>Male</Radio.Button>
                  </Radio.Group>
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
                    Log Up
                  </Button>
                </Form.Item>
              </Form>
            </div>
            {/* <p className="text-align-center">OR</p>
            <Button
              icon={
                <GooglePlusOutlined
                  style={{ fontSize: "26px", color: "#08c" }}
                />
              }
              style={{
                width: "100%",
                height: "auto",
                padding: "10px 0 5px 0",
                boxShadow: "1px 1px 2px rgb(141, 139, 139)",
                display: "flex",
                justifyContent: "center",
                alignContent: "center",
              }}
              loading={loading}
            >
              Continue with Google
            </Button> */}
            <div className="sign-up text-align-center">
              <Link to={"/login"} className="sign-up text-align-center">
                Sign In for an Account.
              </Link>
            </div>
          </div>
        </div>
        <div>
          <br />
        </div>

        <div className="background">
          <div className="background-left">
            <img src={LeftBack} />
          </div>
          <div className="background-right">
            <img src={RightBack} />
          </div>
        </div>
      </div>
    </>
  );
}
