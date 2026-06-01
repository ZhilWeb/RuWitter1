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
import { useParams } from 'react-router';
import CommunityProfileDescription from "../../components/CommunityProfileDescription";


function CommunityProfileById() {
    let params = new URLSearchParams(window.location.search);
    let userIdURL = params.get('id');
    console.log(userIdURL);

    const [user, setUser] = useState([]);
    const [posts, setPosts] = useState([]);
    const [isLikeLoading, setIsLikeLoading] = useState(false);
    const [communitiesCategories, setCommunitiesCategories] = useState([]);
    const [isOwner, setIsOwner] = useState(false);

    const [fetchPosts, isPostsLoading, postError] = useFetching(async (userIdURL) => {
        console.log(posts);

        // console.log(postsPart[0]);
        const personData = await PostService.getCommunityById(userIdURL);
        let postsPart = await PostService.getPostsByCommunityId(userIdURL);
        let avatar = await PostService.getAvatarById(personData.avatarId);
        let isOwnerOfCommunity = await PostService.IsCurrentUserCommunityManager(userIdURL);
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

        const categories = await PostService.getCommunititesCategories();
        console.log(categories);

        // Сохраняем в стейт категорий на будущее (для других нужд компонента)
        

        // 2. Ищем название категории, используя ЛОКАЛЬНУЮ переменную categories

        personData.category = categories.find(cat => cat.id === personData.communityCategoryId)?.name || "Не указана";
        setCommunitiesCategories(categories);
        setUser(personData);
        setPosts(prevPosts => [...prevPosts, ...postsPart]);
        setIsOwner(isOwnerOfCommunity);
        // setLastPostId(personDataPart[personDataPart.length - 1].id)
    });


    useEffect(() => {
        console.log("ef");
        fetchPosts(userIdURL);
    }, [userIdURL]);


    const handleLike = async (post) => {
        if (isLikeLoading) return;

        setIsLikeLoading(true);

        const newHasLike = !post.hasLike;
        try {
            if (post.hasLike) {
                await PostService.deleteLikeByCommunity(post.id);
            } else {
                await PostService.setLikeByCommunity(post.id);
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
        if (window.confirm("Вы уверены, что хотите удалить эту запись?"))
        {
            await PostService.deletePostById(postId);
            window.location.reload();
        }
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

    console.log(user);
    console.log(isOwner);
    return (
        <Layout className={cl.App}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={cl.main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <div className={clid.post_comment_container}>
                        {postError && <h1>Произошла ошибка ${postError}</h1>}
                        <CommunityProfileDescription user={user} anotherUserId={!isOwner} />
                        <DefaultUserPostList posts={posts} isLikeLoading={isLikeLoading} handleLike={handleLike} deletePost={deletePost} anotherUserId={!isOwner} />
                    </div>
                </ContentRuW>
            </Layout>

        </Layout>
    );
}

export default CommunityProfileById;