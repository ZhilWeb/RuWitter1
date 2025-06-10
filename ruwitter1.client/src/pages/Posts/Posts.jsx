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

function Posts() {

    const [posts, setPosts] = useState([]);
    const [users, setUsers] = useState([]);
    const [lastPostId, setLastPostId] = useState(0);
    const [filter, setFilter] = useState({ sort: '', query: '' });
    const [modal, setModal] = useState(false);
    // const sortedAndSearchedPosts = usePosts(posts, filter.sort, filter.query);
    const lastElement = useRef();
    console.log(lastElement);

    const [totalCount, setTotalCount] = useState(0);
    const limit = 10;
    const [totalPages, setTotalPages] = useState(0);
    
    const [page, setPage] = useState(1);


    const [fetchPosts, isPostsLoading, postError] = useFetching(async (lastPostId) => {
        

        let postsPart = await PostService.getPosts(lastPostId);
        setTotalCount(await PostService.getCountOfPosts());
        setTotalPages(getPageCount(totalCount, limit));
        
        // console.log(postsPart[0]);
        let personDataPart = [];
        let avatarDataPart = [];
        for (let post of postsPart) {
            const personData = await PostService.getPersonalDataById(post.userId);
            personDataPart.push(personData);
            const avatar = await PostService.getAvatarById(personData.avatarId);
            console.log(avatar);
            avatarDataPart.push(avatar);
        }
        
        for (let i = 0; i < postsPart.length; i++) {
            postsPart[i].user = personDataPart[i];
            postsPart[i].user.avatar = avatarDataPart[i];

        }

        console.log(postsPart);

        
        setUsers([...users, ...personDataPart]);
        setPosts([...posts, ...postsPart]);
        setLastPostId(personDataPart[personDataPart.length - 1].id)
    });

    useObserver(lastElement, page < totalPages, isPostsLoading, () => setPage(page + 1));


    useEffect(() => {
        // console.log("ef");
        fetchPosts(lastPostId);
    }, [page]);

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
                        <PostList remove={removePost} posts={posts} />
                        <div ref={lastElement} style={{ background: '#FF0000', height: 20 }} />
                        {isPostsLoading
                            && <div style={{ display: 'flex', justifyContent: 'center', marginTop: 50 }}><LoadingOutlined /></div>
                        }
                    </ContentRuW>
                </Layout>

            </Layout>
    );
}

export default Posts;
