import { Menu, Dropdown, Button, message, Space, Tooltip, Avatar, Image } from 'antd';
import { LogoutOutlined, SettingOutlined, UserOutlined, UserSwitchOutlined} from '@ant-design/icons';
import { Link } from 'react-router-dom';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { ACCESS_TOKEN } from '../../../Contains/Config';
import { BASE_URL_IMAGE } from '../../../Contains/ConfigURL';


function Settings(props) {
	const {dataUser} = props;
	let navigate = useNavigate();

	function handleMenuClick(e) {
		switch (e.key) {
			case "2":
				
				navigate('/settings');
				break;
			case "3":
				localStorage.removeItem(ACCESS_TOKEN)
				setTimeout(() => {
					navigate('/login');
				}, 1000);
				break;
			default :
				navigate('/');
				break;
		}
	}

	const menu = (
		<div className='box-user'>
			<div className='box-title'>
				Account
			</div>
			<div className="box-name">
				<div className='avatar_user'>
					<img src={ !dataUser.urlAvatar ? "https://anhdep123.com/wp-content/uploads/2020/11/anh-cute-hoat-hinh.jpg" : BASE_URL_IMAGE+"/"+dataUser.urlAvatar }></img>
				</div>
				<div>
					<h6>{dataUser.firstname + dataUser.lastname}</h6>
					<p>{dataUser.email}</p>
				</div>
			</div>
			<Menu onClick={handleMenuClick}>
			<Menu.Item key="2" icon={<SettingOutlined />}>
				Settings
			</Menu.Item>
			<Menu.Item key="3" icon={<LogoutOutlined />}>
				Logout
			</Menu.Item>
		</Menu>
		</div>
	);


	return (
		<div>
			<Space wrap>
				<Dropdown.Button overlay={menu} placement="bottom" icon={<UserOutlined />}>
				</Dropdown.Button>
			</Space>
		</div>
	);
}

export default Settings;

