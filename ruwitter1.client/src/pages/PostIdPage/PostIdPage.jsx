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
import PostItem from "../../components/PostItem";
import Pagination from "../../components/UI/pagination/Pagination";
import { useObserver } from "../../hooks/useObserver";
import { Layout } from 'antd';
import { LoadingOutlined } from '@ant-design/icons';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import { useParams } from 'react-router';
import cl from "../Posts/Posts.module.css";
import clid from "./PostIdPage.module.css";
import CommentList from "../../components/CommentList";
import CommentForm from "../../components/CommentForm";
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
    const [isPostLikeLoading, setIsPostLikeLoading] = useState(false);
    const [isCommentLikeLoading, setIsCommentLikeLoading] = useState(false);
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
            const postPersonData = post.communityId == null ? await PostService.getPersonalDataById(post.userId) : await PostService.getCommunityById(post.communityId);
            const avatarPost = await PostService.getAvatarById(postPersonData.avatarId);
            const hasLike = await PostService.isSetLikeByCurrentUser(post.id);
            post.user = postPersonData;
            post.user.avatar = avatarPost;
            post.hasLike = hasLike;
            console.log(post);
            setPost(post);


        };

        fetchPostById(postId);




        const fetchComments = async (postId) => {

            // let post = await PostService.getPostById(postId);

            let commentsPart = await PostService.getCommentsByPostId(postId);

            // console.log(postsPart[0]);
            let personDataPart = [];
            let avatarDataPart = [];
            let likesDataPart = [];
            for (let comment of commentsPart) {
                const personData = await PostService.getPersonalDataById(comment.userId);
                personDataPart.push(personData);
                const avatar = await PostService.getAvatarById(personData.avatarId);
                avatarDataPart.push(avatar);
                const hasLike = await PostService.isSetLikeByCurrentUser(postId, comment.id);
                likesDataPart.push(hasLike);
            }

            for (let i = 0; i < commentsPart.length; i++) {
                commentsPart[i].user = personDataPart[i];
                commentsPart[i].user.avatar = avatarDataPart[i];
                commentsPart[i].hasLike = likesDataPart[i];
            }
            console.log(commentsPart);

            setUsers(personDataPart);
            setComments(commentsPart);
            // setLastPostId(personDataPart[personDataPart.length - 1].id)
        };

        fetchComments(postId);
    }, [postId]);

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

    const handlePostLike = async (rendPost) => {
        if (isPostLikeLoading) return;

        setIsPostLikeLoading(true);

        const newHasLike = !rendPost.hasLike;
        try {
            if (rendPost.hasLike && rendPost.communityId != null) {
                await PostService.deleteLikeByCommunity(rendPost.id);
            } else if (!rendPost.hasLike && rendPost.communityId != null) {
                await PostService.setLikeByCommunity(rendPost.id);
            } else if (rendPost.hasLike && rendPost.communityId == null) {
                await PostService.deleteLike(rendPost.id);
            } else if (!rendPost.hasLike && rendPost.communityId == null) {
                await PostService.setLike(rendPost.id);
            }

        }
        finally {
            setIsPostLikeLoading(false);
            setPost({
                ...post,
                hasLike: newHasLike,
            });
        }
    };

    const handleCommentLike = async (comment) => {
        if (isCommentLikeLoading) return;

        setIsCommentLikeLoading(true);

        const newHasLike = !comment.hasLike;
        try {
            if (comment.hasLike && post.communityId != null) {
                await PostService.deleteCommentLikeByCommunity(post.id, comment.id);
            } else if (!comment.hasLike && post.communityId != null) {
                await PostService.setCommentLikeByCommunity(post.id, comment.id);
            } else if (comment.hasLike && post.communityId == null) {
                await PostService.deleteCommentLike(post.id, comment.id);
            } else if (!comment.hasLike && post.communityId == null) {
                await PostService.setCommentLike(post.id, comment.id);
            }

        }
        finally {
            setIsCommentLikeLoading(false);
            setComments(comments =>
                comments.map(rendComment =>
                    rendComment.id === comment.id
                        ? {
                            ...rendComment,
                            hasLike: newHasLike,
                        }
                        : rendComment
                )
            );
        }
    };

    const createComment = async (newComment) => {
        await PostService.createComment(post.id, newComment.body, newComment.files);
        window.location.reload();
    };

    if (!post.id || !post.user)
    {
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
            <Layout>
                <HeaderRuW activeSidebar={ToggleSidebar} />
                <Layout className={cl.main_container}>
                    <SidebarRuW isActive={isActiveSidebar} />
                    <ContentRuW>
                        <div className={clid.post_comment_container }>
                            <PostItem post={post} isLikeLoading={isPostLikeLoading} handleLike={handlePostLike} disableCommentLink={true} />
                            <CommentForm create={createComment} />
                            <CommentList comments={comments} isLikeLoading={isCommentLikeLoading} handleLike={handleCommentLike} />
                        </div>  
                    </ContentRuW>
                </Layout>

            </Layout>
    );
}

export default PostIdPage;
