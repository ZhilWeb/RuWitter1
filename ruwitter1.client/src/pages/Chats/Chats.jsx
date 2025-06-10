import React, { useEffect, useMemo, useRef, useState } from "react";
import "../../App.css";
import { usePosts } from "../../hooks/usePosts";
import { useFetching } from "../../hooks/useFetching";
import PostService from "../../API/PostService";
import { getPageCount } from "../../utils/pages";
import MyButton from "../../components/UI/button/MyButton";
import MyModal from "../../components/UI/MyModal/MyModal";
import PostForm from "../../components/PostForm";
import PostFilter from "../../components/PostFilter";
import PostList from "../../components/PostList";
import Pagination from "../../components/UI/pagination/Pagination";
import { useObserver } from "../../hooks/useObserver";
import { Layout } from 'antd';
import { LoadingOutlined } from '@ant-design/icons';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
// import cl from "./Posts.module.css";
// import { useNavigate } from "react-router";
import AuthorizeView from "../../components/AuthorizeView";
import { Avatar } from "antd";
import { UserOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';

function Chats() {

    const [chats, setChats] = useState([]);
    const [users, setUsers] = useState([]);


    useEffect(() => {
        console.log("ef");
        const fetchChats = async () => {
            const currUserId = await PostService.getCurrUser();

            let chatsPart = await PostService.getChats();
            let usersPart = [];
            let chatsForm = [];
            // console.log(postsPart[0]);
            for (let chat of chatsPart) {
                let newChat = {};
                for (let user of chat.users)
                {
                    if (user.id !== currUserId)
                    {
                        if (user.avatarId !== null)
                        {
                            const avatar = await PostService.getAvatarById(user.avatarId);
                            console.log(avatar);
                            user.avatar = avatar;
                        }
                        
                        usersPart.push(user);
                        newChat.id = chat.id;
                        newChat.user = user;
                        newChat.messages = chat.messages;
                        chatsForm.push(newChat);
                    }
                }
            }
            setUsers(usersPart);
            setChats(chatsForm);
        };

        fetchChats();
    }, []);

    const { Header, Content, Footer, Sider } = Layout;

    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };
    console.log(users);
    console.log(chats);
    // let navigate = useNavigate();
    return (
        <Layout>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    {chats.map(chat =>
                        <div key={chat.id}>
                            <p>{chat.id}</p>
                            {chat.user.avatarId !== null ? <img src={`data:${chat.user.avatar.contentType};base64,${chat.user.avatar.data}`} height="400px" /> : <Avatar size={50} icon={<UserOutlined />} />}
                            <strong>{chat.user.nickname}</strong>
                            {chat.messages.length !== 0 ? <Link to={`/chat?id=${chat.id}`}>{chat.messages[0].body}. {chat.messages[0].publicDate}. {chat.messages[0].isReaded ? "Прочитано" : "Новое"}</Link> : <p>Пусто</p>}
                        </div>
                    )}
                </ContentRuW>
            </Layout>

        </Layout>
    );
}

export default Chats;
