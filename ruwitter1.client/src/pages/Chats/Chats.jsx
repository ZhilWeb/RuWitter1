import React, { useEffect, useState, useMemo } from "react";
import { Layout, Avatar, Input } from "antd";
import { UserOutlined, LoadingOutlined, SearchOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import PostService from "../../API/PostService";
import { useFetching } from "../../hooks/useFetching";
import cl from "./Chats.module.css";
import clposts from "../Posts/Posts.module.css";

function Chats() {
    const [chats, setChats] = useState([]);
    const [isActiveSidebar, setActiveSidebar] = useState(false);
    const [searchQuery, setSearchQuery] = useState(""); // Состояние для строки поиска

    // Используем изолированный метод сервиса
    const [fetchChats, isLoading, error] = useFetching(async () => {
        const enrichedChats = await PostService.getFullChatsList();
        setChats(enrichedChats);
    });

    // Функция фильтрации чатов (вычисляется при изменении списка чатов или строки поиска)
    const filteredChats = useMemo(() => {
        return chats.filter(chat =>
            chat.user?.nickname?.toLowerCase().includes(searchQuery.toLowerCase())
        );
    }, [chats, searchQuery]);

    useEffect(() => {
        fetchChats();
    }, []);

    const ToggleSidebar = () => setActiveSidebar(!isActiveSidebar);
    console.log(chats);
    return (
        <Layout style={{ minHeight: "100vh" }}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clposts.main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <div className={cl.chatsContainer}>
                        <h2 className={cl.pageTitle}>Ваши диалоги</h2>

                        <div className={cl.searchWrapper}>
                            <Input
                                placeholder="Поиск собеседника..."
                                prefix={<SearchOutlined style={{ color: '#bfbfbf' }} />}
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                size="large"
                                allowClear
                                className={cl.searchInput}
                            />
                        </div>

                        {error && <h3 className={cl.error}>Ошибка: {error}</h3>}

                        {isLoading ? (
                            <div className={cl.loader}><LoadingOutlined style={{ fontSize: 40 }} /></div>
                        ) : (
                            <div className={cl.chatList}>
                                    {filteredChats.length > 0 ? (
                                        filteredChats.map(chat => (
                                            <Link to={`/chat?id=${chat.id}`} key={chat.id} className={cl.chatItem}>
                                                <div className={cl.avatarWrapper}>
                                                    {chat.user?.avatar?.data ? (
                                                        <img
                                                            src={`data:${chat.user.avatar.contentType};base64,${chat.user.avatar.data}`}
                                                            className={cl.chatAvatar}
                                                            alt="avatar"
                                                        />
                                                    ) : (
                                                        <Avatar size={54} icon={<UserOutlined />} />
                                                    )}
                                                </div>
                                                <div className={cl.chatInfo}>
                                                    <div className={cl.chatHeader}>
                                                        <span className={cl.opponentName}>
                                                            {chat.user?.nickname || "Пользователь"}
                                                        </span>
                                                        {chat.messages?.[0] && (
                                                            <span className={cl.chatTime}>
                                                                {new Date(chat.messages[0].publicDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                                            </span>
                                                        )}
                                                    </div>
                                                    <div className={cl.chatBody}>
                                                        {chat.messages?.length > 0 ? (
                                                            <p className={`${cl.lastMessage} ${!chat.messages[0].isReaded ? cl.unread : ''}`}>
                                                                {chat.messages[0].body}
                                                            </p>
                                                        ) : (
                                                            <p className={cl.emptyChat}>Нет сообщений</p>
                                                        )}
                                                    </div>
                                                </div>
                                            </Link>
                                        ))
                                    ) : (
                                        <p className={cl.emptyChat}>Ничего не найдено</p>
                                    )}
                            </div>
                        )}
                    </div>
                </ContentRuW>
            </Layout>
        </Layout>
    );
}

export default Chats;