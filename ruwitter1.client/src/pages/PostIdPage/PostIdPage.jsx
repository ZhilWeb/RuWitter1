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
import { useParams } from 'react-router';
// import cl from "./Posts.module.css";

function PostIdPage() {
    let params = new URLSearchParams(window.location.search);
    let postId = params.get('id');
    console.log(postId);
    const [post, setPost] = useState({});
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
    const [comments, setComments] = useState([]);
    const [users, setUsers] = useState([]);
    /*
    const [lastPostId, setLastPostId] = useState(0);

    const [totalCount, setTotalCount] = useState(0);
    const limit = 10;
    const [totalPages, setTotalPages] = useState(0);

    const [page, setPage] = useState(1);
    */
    
    // const [fetchComments, isComLoading, comError] = useFetching();

    useEffect(() => {
        console.log("Fetching post and comments for ID:", postId);
        const fetchPostById = async (postId) => {
            const post = await PostService.getPostById(postId);
            const postPersonData = await PostService.getPersonalDataById(post.userId);
            const avatarPost = await PostService.getAvatarById(postPersonData.avatarId);
            post.user = postPersonData;
            post.user.avatar = avatarPost;
            setPost(post);


        };

        fetchPostById(postId);




        const fetchComments = async (postId) => {

            // let post = await PostService.getPostById(postId);

            let commentsPart = await PostService.getCommentsByPostId(postId);

            // console.log(postsPart[0]);
            let personDataPart = [];
            let avatarDataPart = [];
            for (let comment of commentsPart) {
                const personData = await PostService.getPersonalDataById(comment.userId);
                personDataPart.push(personData);
                const avatar = await PostService.getAvatarById(personData.avatarId);
                avatarDataPart.push(avatar);
            }

            for (let i = 0; i < commentsPart.length; i++) {
                commentsPart[i].user = personDataPart[i];
                commentsPart[i].user.avatar = avatarDataPart[i];

            }


            setUsers(personDataPart);
            setComments(commentsPart);
            // setLastPostId(personDataPart[personDataPart.length - 1].id)
        };

        fetchComments(postId);
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
    console.log(post);
    return (
            <Layout>
                <HeaderRuW activeSidebar={ToggleSidebar} />
                <Layout>
                    <SidebarRuW isActive={isActiveSidebar} />
                    <ContentRuW>
                        <h1>Вы открыли страницу поста с ID={postId}</h1>
                        {post.id !== undefined &&
                            <div>
                                <div>
                                    <img src={`data:${post.user.avatar.contentType};base64,${post.user.avatar.data}`} height="400px" />
                                    <strong>{post.user.nickname}. {post.publicDate}</strong>
                                    <p>{post.body}</p>
                                </div>
                                <div>
                                    {post.mediaFiles.map((mediaFile) =>
                                        <img src={`data:${mediaFile.contentType};base64,${mediaFile.data}`} height="400px" key={mediaFile.id} />
                                    )}
                                </div>
                            </div>
                            
                        }
                        {post.id !== undefined &&
                            <div>
                                {comments.map(comment =>
                                    <div key={comment.id} >
                                        <img src={`data:${comment.user.avatar.contentType};base64,${comment.user.avatar.data}`} height="400px" />
                                        <strong>{comment.user.nickname}. {comment.publicDate}</strong>
                                        <br></br>
                                        <p>{comment.body}</p>
                                        {comment.mediaFiles.map((mediaFile) =>
                                            <img src={`data:${mediaFile.contentType};base64,${mediaFile.data}`} height="400px" key={mediaFile.id} />
                                        )}
                                    </div>
                                )}
                            </div>
                        }  
                    </ContentRuW>
                </Layout>

            </Layout>
    );
}

export default PostIdPage;
