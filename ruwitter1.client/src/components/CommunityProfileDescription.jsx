import React, { useState, useRef } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGear } from "@fortawesome/free-solid-svg-icons";
import cl from "../pages/Posts/Posts.module.css";
import cluser from "../pages/DefaultUserProfile/DefaultUserProfile.module.css";
import { Link } from 'react-router-dom';

const CommunityProfileDescription = ({ user, anotherUserId}) => {
    console.log(user);
    console.log(anotherUserId);
    return (
        <div className={cluser.user_description_container}>
            <div className={cl.post_content}>
                <div className={cluser.avatar_name}>
                    {user.avatar ? <img src={`data:${user.avatar.contentType};base64,${user.avatar.data}`} height="400px" className={cluser.avatar} />
                        : <div style={{ backgroundColor: '#888888', height: '80px', width: '80px' }}></div>}
                    {user.name && <div className={cl.username}>{user.name}</div>}
                </div>
                <div className={cluser.description_body} style={{ height: '100px' }}>
                    {user.category && <div className={cluser.simple_discription}>Категория: <p>{user.category}</p></div>}
                    {user.briefInformation && <div>О себе:<p>{user.briefInformation}</p></div>}
                </div>

                {!anotherUserId &&
                    <div>
                        <Link to={`/changecommunityprofile?id=${user.id}`} className={cluser.post_btns}>
                            {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                            <FontAwesomeIcon icon={faGear} style={{ color: "#1da1f2" }} size="2x" />
                            <div className={cluser.change_profile}>Изменить профиль</div>
                        </Link>
                        <Link to={`/createcommunitypost?id=${user.id}`} className={cluser.post_btns}>
                            {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                            <div className={cluser.change_profile}>Создать запись</div>
                        </Link>
                    </div>}
                    

            </div>

        </div>
    );
}

export default CommunityProfileDescription;