import React, { useEffect, useState, useRef, useMemo } from "react";
import { Layout, Avatar, Input } from "antd";
import { UserOutlined, LoadingOutlined, PaperClipOutlined, SearchOutlined, DeleteOutlined, CheckOutlined } from '@ant-design/icons';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import PostService from "../../API/PostService";
import { useFetching } from "../../hooks/useFetching";
import cl from "./ChatIdPage.module.css";
import clposts from "../Posts/Posts.module.css";
import { Link } from 'react-router-dom';

function ChatIdPage() {
    const params = new URLSearchParams(window.location.search);
    const chatId = params.get('id');

    const [currentUser, setCurrentUser] = useState([]);
    const [chat, setChat] = useState(null);
    const [content, setContent] = useState("");
    const [file, setFile] = useState(null);
    const [messageSearch, setMessageSearch] = useState(""); // Строка поиска сообщений
    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const messagesEndRef = useRef(null);
    const submitRef = useRef(null);

    const [fetchChat, isLoading, error] = useFetching(async (id) => {
        const currUser = await PostService.getCurrUserPersonalData();
        
        // 1. Получаем базовую инфо о чате
        const currentChat = await PostService.getChatById(id);
        // console.log(currentChat);
        // 2. Получаем список сообщений
        const chatMes = await PostService.getMessagesByChatId(id);
        
        
        // Определение оппонента
        const opponentId = currentChat.users[0].id === currUser.userId ? currentChat.users[1].id : currentChat.users[0].id;
        const opponentData = await PostService.getPersonalDataById(opponentId);

        if (opponentData && opponentData.avatarId) {
            opponentData.avatar = await PostService.getAvatarById(opponentData.avatarId);
        }

        // Собираем авторов и файлы для каждого сообщения
        for (let mes of chatMes) {
            // mes.mediaFiles = await PostService.getMediaFilesByMessageId(mes.id);
            mes.user = await PostService.getPersonalDataById(mes.userId);
        }
        currentChat.user = opponentData;
        // Переворачиваем, чтобы старые сообщения были сверху, а новые снизу
        currentChat.messages = chatMes.reverse();

        console.log(currentChat);
        setCurrentUser(currUser);
        setChat(currentChat);
    });

    // Следим за изменением ID чата в строке браузера
    useEffect(() => {
        if (chatId) {
            fetchChat(chatId);
        }
    }, [chatId]);

    // Скролл вниз при появлении чата или новых сообщений
    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [chat?.messages]);

    // Фильтрация сообщений по подстроке
    const filteredMessages = useMemo(() => {
        if (!chat?.messages) return [];
        return chat.messages.filter(msg =>
            msg.body?.toLowerCase().includes(messageSearch.toLowerCase())
        );
    }, [chat?.messages, messageSearch]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!content.trim() && !file) return;

        try {
            // Используем твой метод отправки FormData
            await PostService.sendMessageWithFile(chatId, content, file);
            setContent("");
            setFile(null);
            // Мгновенно обновляем чат
            fetchChat(chatId);
        } catch (err) {
            console.error("Не удалось отправить сообщение:", err);
        }
    };

    // Функция удаления сообщения
    const handleDeleteMessage = async (messageId) => {
        if (window.confirm("Вы уверены, что хотите удалить это сообщение?")) {
            try {
                await PostService.deleteMessageById(messageId);
                // Локально обновляем стейт, чтобы мгновенно убрать сообщение без перезагрузки всей страницы
                setChat(prevChat => ({
                    ...prevChat,
                    messages: prevChat.messages.filter(m => m.id !== messageId)
                }));
            } catch (err) {
                alert("Не удалось удалить сообщение");
            }
        }
    };

    const handleButtonClick = () => {
        submitRef.current.click();
    };

    const ToggleSidebar = () => setActiveSidebar(!isActiveSidebar);

    return (
        <Layout style={{ minHeight: "100vh" }}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clposts.main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <div className={cl.chatWindow}>
                        {isLoading && !chat && <div className={cl.mainLoader}><LoadingOutlined style={{ fontSize: 40 }} /></div>}
                        {error && <h3 className={cl.error}>Ошибка: {error}</h3>}

                        {chat && (
                            <>
                                {/* Шапка текущего диалога */}
                                <div className={cl.chatHeader}>
                                    <Link to={`/user?id=${chat.user.userId}`}>
                                        <div className={cl.userInfo}>
                                            {chat.user?.avatar?.data ? (
                                                <img src={`data:${chat.user.avatar.contentType};base64,${chat.user.avatar.data}`} className={cl.headerAvatar} alt="" />
                                            ) : (
                                                <Avatar size={40} icon={<UserOutlined />} />
                                            )}
                                            <strong className={cl.headerName}>{chat.user?.nickname || "Пользователь"}</strong>
                                        </div>
                                    </Link>

                                    {/* Маленький инпут поиска внутри чата */}
                                    <div className={cl.messageSearchBox}>
                                        <Input
                                            placeholder="Поиск по сообщениям..."
                                            prefix={<SearchOutlined />}
                                            value={messageSearch}
                                            onChange={(e) => setMessageSearch(e.target.value)}
                                            size="small"
                                            allowClear
                                        />
                                    </div>
                                </div>

                                {/* Лента сообщений */}
                                <div className={cl.messagesContainer}>
                                    {filteredMessages.map(msg => {
                                        const isMe = msg.userId === currentUser.userId;
                                        return (
                                            <div key={msg.id} className={`${cl.messageRow} ${isMe ? cl.myMessageRow : cl.opponentMessageRow}`}>
                                                <div className={`${cl.messageBubble} ${isMe ? cl.myBubble : cl.opponentBubble}`}>
                                                    {!isMe && <span className={cl.senderName}>{msg.user?.nickname}</span>}

                                                    <p className={cl.messageText}>{msg.body}</p>

                                                    {msg.mediaFiles?.map(media => (
                                                        <img src={`data:${media.contentType};base64,${media.data}`} className={cl.messageImage} key={media.id} alt="" />
                                                    ))}

                                                    <div className={cl.messageFooter}>
                                                        {/* Кнопка удаления рендерится ТОЛЬКО для твоих сообщений */}
                                                        {isMe && (
                                                            <DeleteOutlined
                                                                className={cl.deleteIcon}
                                                                onClick={() => handleDeleteMessage(msg.id)}
                                                            />
                                                        )}
                                                        <span className={cl.messageTime}>
                                                            {new Date(msg.publicDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        );
                                    })}
                                    <div ref={messagesEndRef} />
                                </div>

                                {/* Нижняя панель отправки */}
                                <form onSubmit={handleSubmit} className={cl.inputForm}>
                                    <input
                                        type="text"
                                        value={content}
                                        onChange={(e) => setContent(e.target.value)}
                                        placeholder="Напишите сообщение..."
                                        className={cl.textInput}
                                    />
                                    <label className={cl.fileInputLabel}>
                                        <PaperClipOutlined style={{ fontSize: 20, cursor: 'pointer', color: file ? '#1890ff' : '#888' }} />
                                        <input
                                            type="file"
                                            onChange={(e) => setFile(e.target.files[0])}
                                            style={{ display: 'none' }}
                                        />
                                    </label>
                                    <button type="submit" className={cl.sendButton} ref={submitRef}>Отправить</button>
                                    <CheckOutlined onClick={handleButtonClick} style={{ fontSize: '20px', color: '#1890ff' }} />
                                </form>
                                {file && <div className={cl.fileBadge}>Прикреплен файл: {file.name}</div>}
                            </>
                        )}
                    </div>
                </ContentRuW>
            </Layout>
        </Layout>
    );
}

export default ChatIdPage;