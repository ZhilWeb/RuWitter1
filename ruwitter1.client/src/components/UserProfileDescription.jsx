import React, { useState, useRef } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGear } from "@fortawesome/free-solid-svg-icons";
import cl from "../pages/Posts/Posts.module.css";
import cluser from "../pages/DefaultUserProfile/DefaultUserProfile.module.css";
import { Link } from 'react-router-dom';

const UserProfileDescription = (user) => {
    console.log(user.user);
    return (
        <div className={cluser.user_description_container}>
            <div className={cl.post_content}>
                <div className={cluser.avatar_name}>
                    {user.user.avatar ? <img src={`data:${user.user.avatar.contentType};base64,${user.user.avatar.data}`} height="400px" className={cluser.avatar} />
                        : <div style={{ backgroundColor: '#888888', height: '80px', width: '80px' }}></div>}
                    {user.user.nickname && <div className={cl.username}>{user.user.nickname}</div>}
                </div>
                <div className={cluser.description_body}>
                    {user.user.phoneNumber && <div className={cluser.simple_discription}>Телефон: <p>{user.user.phoneNumber}</p></div>}
                    {user.user.age && <div className={cluser.simple_discription}>Возраст: <p>{user.user.age}</p></div>}
                    {user.user.city && <div className={cluser.simple_discription}>Город: <p>{user.user.city}</p></div>}
                    {user.user.interests && <div className={cluser.simple_discription}>Интересы: <p>{user.user.interests}</p></div>}
                    {user.user.briefInformation && <div>О себе:<p>{user.user.briefInformation}</p></div>}
                </div>
                
                
                <Link to={`/changeprofile`} className={cluser.post_btns}>
                    {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                    <FontAwesomeIcon icon={faGear} style={{ color: "#1da1f2" }} size="2x" />
                    <div className={cluser.change_profile}>Изменить профиль</div>
                </Link>
            </div>

        </div>
    );

};

export default UserProfileDescription;