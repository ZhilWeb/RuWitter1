import React, { useEffect, useMemo, useRef, useState } from "react";
import { Layout } from 'antd';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import cl from "../Posts/Posts.module.css";
import clpost from "../CreatePost/CreatePost.module.css";
import PostService from "../../API/PostService";
import PostForm from "../../components/PostForm";
import { useParams } from 'react-router';

function UpdatePost() {
    let params = new URLSearchParams(window.location.search);
    let postId = params.get('id');
    console.log(postId);
    const [post, setPost] = useState({});
    useEffect(() => {
        console.log("Fetching post and comments for ID:", postId);
        const fetchPostById = async (postId) => {
            const post = await PostService.getPostById(postId);
            console.log(post);
            setPost(post);
        };

        fetchPostById(postId);
    }, [postId]);


    const { Header, Content, Footer, Sider } = Layout;


    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };

    const updatePost = async (newPost) => {
        await PostService.updatePost(post.id, newPost.body, newPost.files);
        alert("Запись обновлена.");
        window.location.href = "/profile";
    };

    return (
        <Layout className={cl.App}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clpost.post_main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <PostForm create={updatePost} filledBody={post.body} />
                </ContentRuW>
            </Layout>

        </Layout>
    );
}

export default UpdatePost;