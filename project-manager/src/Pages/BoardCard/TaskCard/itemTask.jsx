import { DeleteOutlined, UserAddOutlined } from '@ant-design/icons';
import { Checkbox } from 'antd';
import { useEffect, useState } from 'react';
import { BASE_URL_IMAGE } from '../../../Contains/ConfigURL';
import { apiClient } from '../../../Services';
const ItemCard = (props) => {
    const { onChange, _i, i, handleRemoveTask, memberCard } = props;
    const [isShowMember, setIsShowMember] = useState(false);
    const [oneMemberTask, setOneMemberTask] = useState("");

    useEffect(() => {
        apiClient.fetchApiGetTaskUserMenbers(i.id).then(res => {
            if (res.data) {
                setOneMemberTask(res.data);
            } else {
                // console.log("Fail.....")
            }
        })
    }, [])

    const handleRemoveTaskList = () => {
        handleRemoveTask(_i);
    }

    const handleMemberTaskList = () => {
        setIsShowMember(true);
    }

    const handlTaskListMember = (user) => {
        console.log("Task:" , i , "User:", user);
        if(oneMemberTask && oneMemberTask !== null){
            apiClient.fetchApiDeleteTaskUserMenber(i.id, "fef5731b-9b86-4805-8717-2e01ed217515").then(res => {
                // console.log(res.data);
            })
        }
        apiClient.fetApiCreateTaskUserMenber(i.id, "fef5731b-9b86-4805-8717-2e01ed217515")
        setOneMemberTask(user);
    }


    return (
        <>
            <div className='footer-task d-flex task-drag-handle'>
                <Checkbox
                    onChange={(e) => onChange(e, _i)}
                    checked={i.isActive}
                ></Checkbox>
                <p className={`task-name ${i.isActive ? 'unline' : ''}`}>{i.name}</p>
                <div className='remove-task'>
                    <UserAddOutlined style={{ marginRight: '5px' }} onClick={handleMemberTaskList} />
                    <DeleteOutlined onClick={handleRemoveTaskList} />
                </div>
                {
                    isShowMember && (
                        <div className='list-member-card'>
                            <div className='header-list d-flex'>
                                <h6 className='header-title'>Member</h6>
                                <span className='close-list-member' onClick={() => setIsShowMember(false)}>x</span>
                            </div>
                            <div className='body-list'>
                                <h6 className='body-title'>Board members</h6>
                                {
                                    memberCard.length > 0 && memberCard.map((item, _i) => (
                                        <div className='body-list-member' key={_i} onClick={() => handlTaskListMember(item)}>
                                            <div className='member__item'>
                                                <div className='item__avatar'>
                                                    <img src={!item.urlAvatar ? "https://anhdep123.com/wp-content/uploads/2020/11/anh-cute-hoat-hinh.jpg" : BASE_URL_IMAGE + "/" + item.urlAvatar} />
                                                </div>
                                                <div className='item__name'>
                                                    <p className='item__name-user'>
                                                        {item.firstname + item.lastname}
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    ))
                                }
                            </div>
                        </div>
                    )
                }
            </div>
            <p>{oneMemberTask.email}</p>
        </>
    )
}

export default ItemCard;