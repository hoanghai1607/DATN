import { Avatar, Divider, Tooltip } from "antd";
import { UserOutlined, AntDesignOutlined } from "@ant-design/icons";
import "./GroundAvatar.scss";
import { BASE_URL_IMAGE } from "../../Contains/ConfigURL";
import ResumeUser from "../../Pages/ResumeUser";
const GroundAvatar = (props) => {
  const { member, size = "large" } = props;
  const handleShowUser = (i) => {
    window.location.replace(`/resume-user/${i.id}`);
  };
  return (
    <>
      <Avatar.Group
        maxCount={2}
        size={size}
        maxStyle={{ color: "#f56a00", backgroundColor: "#fde3cf" }}
      >
        {member &&
          member.length > 0 &&
          member.map((i, _i) => (
            <Tooltip
              title={i.firstname + " " + i.lastname}
              placement="top"
              key={_i}
              onClick={() => handleShowUser(i)}
            >
              <Avatar
                src={
                  !i.urlAvatar
                    ? "https://static.vecteezy.com/system/resources/previews/009/292/244/non_2x/default-avatar-icon-of-social-media-user-vector.jpg"
                    : BASE_URL_IMAGE + "/" + i.urlAvatar
                }
              />
            </Tooltip>
          ))}
      </Avatar.Group>
    </>
  );
};

export default GroundAvatar;
