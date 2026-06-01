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
import cl from "../Posts/Posts.module.css";
import clid from "../PostIdPage/PostIdPage.module.css";
// import { useNavigate } from "react-router";
import AuthorizeView from "../../components/AuthorizeView";
import PostItem from "../../components/PostItem";
import UserProfileDescription from "../../components/UserProfileDescription";
import DefaultUserPostList from "../../components/DefaultUserPostList";


function DefaultUserProfile() {
    const [user, setUser] = useState([]);
    const [posts, setPosts] = useState([]);
    const [isLikeLoading, setIsLikeLoading] = useState(false);

    const [fetchPosts, isPostsLoading, postError] = useFetching(async () => {
        console.log(posts);

        // console.log(postsPart[0]);
        const personData = await PostService.getCurrUserPersonalData();
        let postsPart = await PostService.getCurrentUserPosts();
        let avatar = await PostService.getAvatarById(personData.avatarId);
        personData.avatar = avatar;
        let likesDataPart = [];
        for (let post of postsPart) {
            const hasLike = await PostService.isSetLikeByCurrentUser(post.id);
            likesDataPart.push(hasLike);
        }

        for (let i = 0; i < postsPart.length; i++) {
            postsPart[i].user = personData;
            postsPart[i].user.avatar = avatar;
            postsPart[i].hasLike = likesDataPart[i];
        }
        console.log(personData);
        console.log(postsPart);

        setUser(personData);
        setPosts(prevPosts => [...prevPosts, ...postsPart]);
        // setLastPostId(personDataPart[personDataPart.length - 1].id)
    });


    useEffect(() => {
        console.log("ef");
        fetchPosts();
    }, []);


    const handleLike = async (post) => {
        if (isLikeLoading) return;

        setIsLikeLoading(true);

        const newHasLike = !post.hasLike;
        try {
            if (post.hasLike && post.communityId == null) {
                await PostService.deleteLike(post.id);
            } else if (!post.hasLike && post.communityId == null) {
                await PostService.setLike(post.id);
            }

        }
        finally {
            setIsLikeLoading(false);
            setPosts(posts =>
                posts.map(rendPost =>
                    rendPost.id === post.id
                        ? {
                            ...rendPost,
                            hasLike: newHasLike,
                        }
                        : rendPost
                )
            );
        }
    };

    const deletePost = async (postId) => {
        await PostService.deletePostById(postId);
        window.location.reload();
    };
    
    const { Header, Content, Footer, Sider } = Layout;

    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };
    if (!user && !posts[0].id) {
        return (
            <Layout>
                <HeaderRuW activeSidebar={ToggleSidebar} />
                <Layout>
                    <SidebarRuW isActive={isActiveSidebar} />
                    <ContentRuW>
                        <div style={{ display: 'flex', justifyContent: 'center', marginTop: 50 }}><LoadingOutlined /></div>
                    </ContentRuW>
                </Layout>

            </Layout>
        );
    }

    
    return (
        <Layout className={cl.App}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={cl.main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <div className={clid.post_comment_container}>
                        {postError && <h1>Произошла ошибка ${postError}</h1>}
                        <UserProfileDescription user={user} />
                        <DefaultUserPostList posts={posts} isLikeLoading={isLikeLoading} handleLike={handleLike} deletePost={deletePost} />
                    </div>
                </ContentRuW>
            </Layout>

        </Layout>
    );
}

export default DefaultUserProfile;