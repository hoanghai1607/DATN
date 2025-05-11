import Modal from "antd/lib/modal/Modal";
import React, { useState } from "react";
import { Input, Select } from "antd";
import { BACKGROUD_BOARD } from "../../actions/dataBackgroud";
function CreateBoardModal({ handleChangeTitle, handleChangeNumber, isModalVisible, handleOk, handleCancel, handleChangeBackGround}) {
	const [background , setBackground] = useState(BACKGROUD_BOARD[0]);
	const [indexBg, setIndexBg] = useState(0);

	const handleBackGround = (index) => {
		handleChangeBackGround(index);
	}
	return (
		<div>
			<Modal
				title="Create a new board"
				visible={isModalVisible}
				onOk={handleOk}
				onCancel={handleCancel}
			>
				<div className="show-image">
					<img src={background}/>
				</div>
				<div className="gr__backgroud-api">
					{
						BACKGROUD_BOARD.map((i, _i) => (
							<div className="bg-board-new" key={_i} onClick={() => {setBackground(i); setIndexBg(_i) ; handleBackGround(_i)}}>
								<img src={i} />
							</div>
						))
					}
				</div>
				Board Title
				<Input placeholder="Basic usage" onChange={handleChangeTitle} />
				Members Number
				<Input placeholder="Basic usage" onChange={handleChangeNumber} />
			</Modal>
		</div>
	);
}

export default CreateBoardModal;
