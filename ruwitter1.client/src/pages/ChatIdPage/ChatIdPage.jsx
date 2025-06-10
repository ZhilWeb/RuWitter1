import React, { useEffect, useState } from "react";
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
import { useParams } from 'react-router';
import { Avatar } from "antd";
import { UserOutlined } from '@ant-design/icons';

// import cl from "./Posts.module.css";

function ChatIdPage() {
    let params = new URLSearchParams(window.location.search);
    let chatId = params.get('id');
    console.log(chatId);
    const [chat, setChat] = useState({});
    // const [attempts, setAttempts] = useState(0);
    /*
    const [fetchPostById, isLoading, error] = useFetching(async (postId) => {
        const post = await PostService.getPostById(postId);
        console.log(post);
        const postPersonData = await PostService.getPersonalDataById(post.userId);
        console.log(postPersonData);
        const avatarPost = await PostService.getAvatarById(postPersonData.avatarId);
        console.log(avatarPost);
        post.user = postPersonData;
        post.user.avatar = avatarPost;
        setPost(post);

        
    });
    */
    const [currUserId, setCurrUserId] = useState({});
    const [anotherUser, setAnotherUser] = useState({});
    /*
    const [lastPostId, setLastPostId] = useState(0);

    const [totalCount, setTotalCount] = useState(0);
    const limit = 10;
    const [totalPages, setTotalPages] = useState(0);

    const [page, setPage] = useState(1);
    */

    // const [fetchComments, isComLoading, comError] = useFetching();

    useEffect(() => {
        console.log("ef");
        const fetchChats = async (chatId) => {
            const currUserId = await PostService.getCurrUser();
            setCurrUserId(currUserId);

            let chat = await PostService.getChatById(chatId);
            let messagesWithFiles = await PostService.getMessagesByChatId(chat.id);
            if (chat.messages.length > 0) {
                for (let i = 0; i < chat.messages.length; i++) {
                    chat.messages[i].mediaFiles = messagesWithFiles[i].mediaFiles;
                }
            }
            let anotherUser = {};
            let newChat = {};
            // console.log(postsPart[0]);
            for (let user of chat.users) {
                if (user.id !== currUserId) {
                    if (user.avatarId !== null) {
                        const avatar = await PostService.getAvatarById(user.avatarId);
                        console.log(avatar);
                        user.avatar = avatar;
                    }

                    anotherUser = user;
                    newChat.id = chat.id;
                    newChat.user = user;
                    newChat.messages = chat.messages;
                }
            }
            
            setAnotherUser(anotherUser);
            setChat(newChat);
        };

        fetchChats(chatId);
    }, []);

    /*
    const createPost = (newPost) => {
        setPosts([...posts, newPost])
        setModal(false)
    };


    const removePost = (post) => {
        setPosts(posts.filter(p => p.id !== post.id))
    };

    const changePage = (page) => {
        setPage(page);
    };
    */

    const { Header, Content, Footer, Sider } = Layout;

    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };

    
    console.log(chat);
    console.log(anotherUser);
    console.log(currUserId);

    const [content, setContent] = useState('');
    const [file, setFile] = useState(null);
    const [messageId, setMessageId] = useState('');
    const [fileInfo, setFileInfo] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log(content);
        console.log(file);
        console.log(chat.id);
        const result = await PostService.sendMessageWithFile(content, file, chat.id);
        // setMessageId(result.id);
        alert('Message with file saved successfully!');
    };

    return (
        <Layout>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <h1>Вы открыли страницу чата с ID={chatId}</h1>
                    {chat.id !== undefined &&
                        <div>
                            <img src={`data:${chat.user.avatar.contentType};base64,${chat.user.avatar.data}`} height="400px" />
                            <strong>{chat.user.nickname}</strong>
                            {chat.messages.map(chatMes =>
                                <div key={chatMes.id}>

                                    <strong>{chatMes.user.nickname}</strong>
                                    <p >{chatMes.body}. {chatMes.publicDate}</p>
                                    {chatMes.mediaFiles.map(mediaFile =>
                                        <img src={`data:${mediaFile.contentType};base64,${mediaFile.data}`} height="100px" key={mediaFile.id} />
                                    )}
                                </div>
                            )}
                        </div>
                    }

                    <h2>Send Message with Attachment</h2>
                    <form onSubmit={handleSubmit}>
                        <textarea
                            value={content}
                            onChange={(e) => setContent(e.target.value)}
                            placeholder="Enter your message"
                            required
                        />
                        <input
                            type="file"
                            onChange={(e) => setFile(e.target.files[0])}
                        />
                        <button type="submit">Encrypt and Save</button>
                    </form>
                    
                </ContentRuW>
            </Layout>

        </Layout>
    );
}

export default ChatIdPage;
