import React, { useState, useRef } from "react";
import MyButton from "./UI/button/MyButton";
import MyInput from "./UI/input/MyInput";
import clform from "./UI/CommentFormStyle/CommentForm.module.css";
import plform from "./UI/PostFormStyle/PostFormStyle.module.css";
import userform from "./UI/UpdateProfileStyle/UpdateProfile.module.css";
import { Layout, Avatar, Card, Row, Col, Input, InputNumber, Button, Space, Select } from "antd";
import { PaperClipOutlined, CheckOutlined } from '@ant-design/icons';
import cluser from "../pages/DefaultUserProfile/DefaultUserProfile.module.css";
import { Link } from 'react-router-dom';


const UpdateCommunityProfile = ({ update, communitiesCategories, communityId, buttonText }) => {
    const [personData, setPersonData] = useState({
        name: "",
        briefInformationSubstring: "",
        communityCategoryId: 0
    });

    const [avatar, setAvatar] = useState(null);

    const newUpdateProfile = (e) => {
        e.preventDefault()
        const newProfile = {
            ...personData
        }
        const newAvatar = avatar;

        if (newProfile.age < 1 || newProfile.age > 100) {
            return;
        }

        console.log(newProfile);
        console.log(newAvatar);

        update(newProfile, newAvatar);
    }

    const [imageSrc, setImageSrc] = useState(null);

    const handleFileChange = (event) => {
        const chosenFile = event.target.files[0];
        setAvatar(chosenFile);
        const file = event.target.files[0];
        if (file) {
            // Создаем временную ссылку на файл для тега img
            const objectUrl = URL.createObjectURL(file);
            setImageSrc(objectUrl);
        }
    };


    return (
        <form className={plform.post_form}>
            {/*Управляемый компонент*/}
            <div className={plform.post_inputs}>
                {
                    imageSrc ?
                        <img src={imageSrc} style={{ height: '80px', width: '80px' }} className={cluser.avatar} /> :
                        <div style={{ backgroundColor: '#888888', height: '80px', width: '80px' }}></div>
                }
                <MyInput
                    name='avatar'
                    type="file"
                    onChange={handleFileChange}
                    accept="image/*"
                    style={{ paddingLeft: 0 }}
                />
                <MyInput
                    value={personData.name}
                    name='name'
                    onChange={e => setPersonData({ ...personData, name: e.target.value })}
                    type="text"
                    placeholder="Название"
                    style={{ paddingLeft: 0, borderTop: '2px solid #1DA1F2' }}
                />
                <Col xs={24} sm={12} md={8}>
                    <Select
                        showSearch // Включает текстовый поиск прямо внутри селекта
                        allowClear
                        style={{ width: '100%' }}
                        placeholder="Поиск категорий по названию..."
                        // Связываем со значением в стейте (это будет массив ID, например [1, 2])
                        value={personData.communityCategoryId}
                        // Когда пользователь выбирает категорию, Ant Design передает массив выбранных values (id)
                        onChange={selectedId => setPersonData({ ...personData, communityCategoryId: selectedId })}
                        // Логика фильтрации: ищем совпадение текста, введенного пользователем, с названием категории
                        filterOption={(input, option) =>
                            option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                        }
                    >
                        {/* Рендерим опции: для пользователя показываем name, а в value прячем id */}
                        {communitiesCategories.map(cat => (
                            <Option key={cat.id} value={cat.id}>
                                {cat.name}
                            </Option>
                        ))}
                    </Select>
                </Col>
                <textarea
                    value={personData.briefInformation}
                    name='briefInformation'
                    onChange={e => setPersonData({ ...personData, briefInformation: e.target.value })}
                    type="text"
                    placeholder="Напишите что-нибудь..."
                    maxLength="10000"
                    // rows="10"
                    // cols="50"
                    className={plform.post_form_body}
                    style={{ borderBottom: '2px solid #1DA1F2' }}
                ></textarea>
                
            </div>
            <div style={{ display: 'flex', width: '520px', justifyContent: 'space-between' }}>
                {communityId && <Link to={`/community?id=${communityId}`} ><MyButton style={{ backgroundColor: '#FFFFFF', color: '#1DA1F2' }}>Отменить</MyButton></Link>}
                <MyButton onClick={newUpdateProfile}>{buttonText}</MyButton>
            </div>

        </form>
    );

};

export default UpdateCommunityProfile;