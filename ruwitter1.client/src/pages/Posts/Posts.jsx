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
import cl from "./Posts.module.css";
// import { useNavigate } from "react-router";
import AuthorizeView from "../../components/AuthorizeView";
import PostItem from "../../components/PostItem";

function Posts() {

    const [posts, setPosts] = useState([]);
    const [users, setUsers] = useState([]);
    // const [lastPostId, setLastPostId] = useState(0);
    const [filter, setFilter] = useState({ sort: '', query: '' });
    const [modal, setModal] = useState(false);
    const [isLikeLoading, setIsLikeLoading] = useState(false);
    // const sortedAndSearchedPosts = usePosts(posts, filter.sort, filter.query);
    const lastElement = useRef();
    console.log(lastElement);
    
    // const [page, setPage] = useState(1);


    const [fetchPosts, isPostsLoading, postError] = useFetching(async () => {
        console.log(posts);

        let postsPart = await PostService.getPosts();
        
        // console.log(postsPart[0]);
        let personDataPart = [];
        let avatarDataPart = [];
        let likesDataPart = [];
        for (let post of postsPart) {
            const personData = await PostService.getCommunityById(post.communityId);
            personDataPart.push(personData);
            // console.log(personData);
            const avatar = await PostService.getAvatarById(personData.avatarId);
            // console.log(personData.avatar);
            avatarDataPart.push(avatar);
            const hasLike = await PostService.isSetLikeByCurrentUser(post.id);
            likesDataPart.push(hasLike);
        }
        
        for (let i = 0; i < postsPart.length; i++) {
            postsPart[i].user = personDataPart[i];
            postsPart[i].user.avatar = avatarDataPart[i];
            postsPart[i].hasLike = likesDataPart[i];
        }

        console.log(postsPart);

        setUsers(prevUsers => [...prevUsers, ...personDataPart]);
        setPosts(prevPosts => [...prevPosts, ...postsPart]);
        // setLastPostId(personDataPart[personDataPart.length - 1].id)
    });

    useObserver(lastElement, posts.length > 0, isPostsLoading, fetchPosts);


    useEffect(() => {
        console.log("ef");
        fetchPosts();
    }, []);

    const createPost = (newPost) => {
        setPosts([...posts, newPost])
        setModal(false)
    };


    const removePost = (post) => {
        setPosts(posts.filter(p => p.id !== post.id))
    };

    

    const handleLike = async (post) => {
        if (isLikeLoading) return;

        setIsLikeLoading(true);

        const newHasLike = !post.hasLike;
        try {
            if (post.hasLike) {
                await PostService.deleteLikeByCommunity(post.id);
            } else
            {
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

    // const changePage = (page) => {
    //     setPage(page);
    // };

    const { Header, Content, Footer, Sider } = Layout;

    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };

    // let navigate = useNavigate();
    return (
            <Layout className={cl.App}>
                <HeaderRuW activeSidebar={ToggleSidebar} />
                <Layout className={cl.main_container}>
                    <SidebarRuW isActive={isActiveSidebar} />
                    <ContentRuW>
                        {postError && <h1>Произошла ошибка ${postError}</h1>}
                        <PostList posts={posts} lastElement={lastElement} isLikeLoading={isLikeLoading} handleLike={handleLike} />
                        
                        {isPostsLoading
                            && <div style={{ display: 'flex', justifyContent: 'center', marginTop: 50 }}><LoadingOutlined /></div>
                        }
                    </ContentRuW>
                </Layout>

            </Layout>
    );
}

export default Posts;
