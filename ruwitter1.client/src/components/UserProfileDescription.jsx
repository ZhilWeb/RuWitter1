import React, { useState, useRef } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGear } from "@fortawesome/free-solid-svg-icons";
import cl from "../pages/Posts/Posts.module.css";
import cluser from "../pages/DefaultUserProfile/DefaultUserProfile.module.css";
import { Link } from 'react-router-dom';

const UserProfileDescription = ({ user, anotherUserId, createChatByUserId, chatId }) => {
    console.log(user);
    console.log(anotherUserId);
    console.log(chatId);
    return (
        <div className={cluser.user_description_container}>
            <div className={cl.post_content}>
                <div className={cluser.avatar_name}>
                    {user.avatar ? <img src={`data:${user.avatar.contentType};base64,${user.avatar.data}`} height="400px" className={cluser.avatar} />
                        : <div style={{ backgroundColor: '#888888', height: '80px', width: '80px' }}></div>}
                    {user.nickname && <div className={cl.username}>{user.nickname}</div>}
                </div>
                <div className={cluser.description_body}>
                    {user.phoneNumber ? <div className={cluser.simple_discription}>Телефон: <p>{user.phoneNumber}</p></div> : <div className={cluser.simple_discription}>Телефон:</div>}
                    {user.age ? <div className={cluser.simple_discription}>Возраст: <p>{user.age}</p></div> : <div className={cluser.simple_discription}>Возраст:</div>}
                    {user.city ? <div className={cluser.simple_discription}>Город: <p>{user.city}</p></div> : <div className={cluser.simple_discription}> Город:</div>}
                    {user.interests ? <div className={cluser.simple_discription}>Интересы: <p>{user.interests}</p></div> : <div className={cluser.simple_discription}> Интересы:</div>}
                    {user.briefInformation ? <div>О себе:<p>{user.briefInformation}</p></div> : <div className={cluser.simple_discription}>О себе:</div>}
                </div>
                
                {anotherUserId ? (chatId !== 0 ?
                    <Link to={`/chat?id=${chatId}`} className={cluser.post_btns}>
                        {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                        <div className={cluser.change_profile}>Перейти к чату</div>
                    </Link> :
                    <div className={cluser.post_btns} onClick={createChatByUserId} style={{ color: '#1DA1F2', cursor: 'pointer' }}>
                        <div className={cluser.change_profile}>Создать чат</div>
                    </div>
                ) :
                    <div>
                        <Link to={`/changeprofile`} className={cluser.post_btns}>
                            {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                            <FontAwesomeIcon icon={faGear} style={{ color: "#1da1f2" }} size="2x" />
                            <div className={cluser.change_profile}>Изменить профиль</div>
                        </Link>
                        <Link to={`/mycommunities`} className={cluser.post_btns}>
                            {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                            <div className={cluser.change_profile}>Мои сообщества</div>
                        </Link>
                    </div>}
                
            </div>

        </div>
    );

};

export default UserProfileDescription;