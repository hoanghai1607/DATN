import { Space, Input } from "antd";
import React, { Component, useState } from "react";
import "./Menu.scss";
import { AudioOutlined } from "@ant-design/icons";
const SearchComponent = (props) => {
	const { Search } = Input;
	const { onSearch, onFocus, onBlur} = props;

	const suffix = (
		<AudioOutlined
			style={{
				fontSize: 16,
				color: "#1890ff",
			}}
		/>
	);
	return (
		<>
			<Space direction="vertical" >
				<Search
					className="search-bar"
					placeholder="Enter Search Board.."
					onSearch={onSearch}
					suffix={suffix}
					onFocus={onFocus}
				/>

			</Space>
		</>
	);
};

export default SearchComponent;
