import { Avatar, Divider, Tooltip } from 'antd';
import { UserOutlined, AntDesignOutlined } from '@ant-design/icons';
import './GroundAvatar.scss';
import { BASE_URL_IMAGE } from '../../Contains/ConfigURL';
import ResumeUser from '../../Pages/ResumeUser';
const GroundAvatar = (props) => {
    const {member} = props;
    const handleShowUser = (i) => {
       window.location.replace(`/resume-user/${i.id}`)
    }
    return (
        <>
            <Avatar.Group
                maxCount={2}
                size="large"
                maxStyle={{ color: '#f56a00', backgroundColor: '#fde3cf' }}
            >
                {
                    member && member.length > 0 && member.map((i, _i) => (
                        <Tooltip title={i.firstname + i.lastname} placement="top" key={_i} onClick={() => handleShowUser(i)}>
                            <Avatar src={!i.urlAvatar ? "https://anhdep123.com/wp-content/uploads/2020/11/anh-cute-hoat-hinh.jpg" : BASE_URL_IMAGE+"/"+i.urlAvatar} />
                        </Tooltip>
                    ))
                }

            </Avatar.Group>
        </>
    )
}

export default GroundAvatar;