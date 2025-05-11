import { SisternodeOutlined } from "@ant-design/icons";
import { Button, Progress, Input} from "antd";
import ItemCard from "./itemTask";
import { Container, Draggable } from "react-smooth-dnd";
import { useState, useEffect, useRef } from "react";
import { PlusCircleOutlined, CloseOutlined } from "@ant-design/icons";
import { apiClient } from "../../../Services";

const TaskCard = (props) => {
    const { percent, onChange, tasks, onTaskListDrop, handleAddTaskNew, card, handleRemoveTask, memberCard} = props;
    const [isShowForm, setIsShowForm] = useState(false);
    const [valueTask, setValueTask] = useState("");
    const inputRef = useRef(null);

    useEffect(() => {
        if (isShowForm === true && inputRef && inputRef.current) {
          inputRef.current.focus();
        }
      }, [isShowForm]);


      const handleCreateTask = () => {
        let data = {
            name: valueTask,
            numberMember: 5,
            icon: "Z",
            comment: "Duy Phương",
            timeExpiry: "2022-07-07T06:14:16.538Z",
            isActive: false,
            order: tasks.length <= 0 ? 1 : ++tasks.length 
        }
        apiClient.fetApiCreateTask(card.id, data).then(res => {
            if(res.data){
                handleAddTaskNew(res.data);
                setValueTask("");
                setIsShowForm(false)
            }else{
                console.log("0");
            }
        })

      }

    

    return (
        <div className='task-checklist'>
            <div className='header-task d-flex justify-content-between align-items-center'>
                <div className='checklist-title'>
                    <SisternodeOutlined /> <span className='name-default'>Checklist</span>
                </div>
                {/* <div className='checklist-delete'>
                    <Button >Delete</Button>
                </div> */}
            </div>
            <div className='body-task mb-2'>
                <Progress percent={percent} />
            </div>
            <Container
                // orientation="horizontal"
                groupName="col"
                onDrop={dropResult => onTaskListDrop(dropResult)}
                dragClass="card-ghost"
                dragHandleSelector=".task-drag-handle"
                dropClass="card-ghost-drop"
                getChildPayload={index =>
                    tasks[index]
                }
                dropPlaceholder={{
                    animationDuration: 150,
                    showOnTop: true,
                    className: 'card-drop-preview'
                }}
                dropPlaceholderAnimationDuration={200}
            >
                {
                    tasks.map((i, _i) => (
                        <Draggable key={_i}>
                            <ItemCard i={i} onChange={onChange} _i={_i} handleRemoveTask={() => handleRemoveTask(_i)} memberCard={memberCard}/>
                        </Draggable>
                    ))
                }

            </Container>

            {!isShowForm ? (
                <div className="mt-2">
                    <Button onClick={() => setIsShowForm(true)}>
                        <span className="icon_plus">
                            <PlusCircleOutlined />
                        </span>{" "}
                        Add Task
                    </Button>
                </div>
            ) : (
                <div className="add-content-list">
                    <Input.Group compact>
                        <Input
                            style={{ width: "calc(100%)" }}
                            placeholder="Enter content.."
                            ref={inputRef}
                            value={valueTask}
                            onChange={(e) => setValueTask(e.target.value)}
                        />
                        <div className="gr_biit">
                            <Button type="primary" onClick={handleCreateTask}>
                                Add Task
                            </Button>
                            <CloseOutlined onClick={() =>{ setIsShowForm(false) ; setValueTask('')}} />
                        </div>
                    </Input.Group>
                </div>
            )}
        </div>
    )
}

export default TaskCard;