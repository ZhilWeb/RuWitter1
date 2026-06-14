import React, { useState, useRef } from "react";
import MyButton from "./UI/button/MyButton";
import MyInput from "./UI/input/MyInput";
import clform from "./UI/CommentFormStyle/CommentForm.module.css";
import plform from "./UI/PostFormStyle/PostFormStyle.module.css";
import userform from "./UI/UpdateProfileStyle/UpdateProfile.module.css";
import { PaperClipOutlined, CheckOutlined } from '@ant-design/icons';
import cluser from "../pages/DefaultUserProfile/DefaultUserProfile.module.css";
import { Link } from 'react-router-dom';


const UpdateProfile = ({ update }) => {

    const [personData, setPersonData] = useState({
        nickname: '',
        phoneNumber: '',
        age: 1,
        city: '',
        interests: '',
        briefInformation: ''
    });

    const [avatar, setAvatar] = useState(null);

    const newUpdateProfile = (e) => {
        e.preventDefault()
        const newProfile = {
            ...personData
        }
        const newAvatar = avatar;

        if (newProfile.age < 1 || newProfile.age > 100)
        {
            return ;
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
        <div className={plform.post_form_container }>
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
                    value={personData.nickname}
                    name='nickname'
                    onChange={e => setPersonData({ ...personData, nickname: e.target.value })}
                    type="text"
                    placeholder="Имя"
                    style={{ paddingLeft: 0, borderTop: '2px solid #1DA1F2' }}
                />
                <MyInput
                    value={personData.phoneNumber}
                    name='phoneNumber'
                    onChange={e => setPersonData({ ...personData, phoneNumber: e.target.value })}
                    type="tel"
                    placeholder="Номер телефона"
                    style={{ paddingLeft: 0, borderTop: '2px solid #1DA1F2', borderBottom: '2px solid #1DA1F2' }}
                />
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
                    style={{ borderBottom: '2px solid #1DA1F2'} }
                ></textarea>
                <label htmlFor="age" style={{fontSize: '20px'} }>Возраст:</label>
                <MyInput
                    value={personData.age}
                    name='age'
                    onChange={e => setPersonData({ ...personData, age: e.target.value })}
                    type="number"
                    min="1"
                    max="100"
                    style={{ paddingLeft: 0 }}
                />
                <MyInput
                    value={personData.city}
                    name='city'
                    onChange={e => setPersonData({ ...personData, city: e.target.value })}
                    type="text"
                    placeholder="Город"
                    maxLength="300"
                    style={{ paddingLeft: 0, borderTop: '2px solid #1DA1F2' }}
                />
                <MyInput
                    value={personData.interests}
                    name='interests'
                    onChange={e => setPersonData({ ...personData, interests: e.target.value })}
                    type="text"
                    placeholder="Интересы"
                    style={{ paddingLeft: 0, borderTop: '2px solid #1DA1F2' }}
                />
            </div>
            <div style={{display: 'flex', width: '520px', justifyContent: 'space-between'} }>
                <Link to={'/profile'} ><MyButton style={{ backgroundColor: '#FFFFFF', color: '#1DA1F2' }}>Отменить</MyButton></Link>
                <MyButton onClick={newUpdateProfile}>Изменить</MyButton>
            </div>
            
            </form>
        </div>
    );

};

export default UpdateProfile;