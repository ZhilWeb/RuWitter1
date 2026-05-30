import { Link } from 'react-router-dom';
import cl from "../pages/Posts/Posts.module.css";
import { format } from 'date-fns';
import { ru } from 'date-fns/locale';

const CommentItem = (props) => {
    // let router = useNavigate();

    const date = new Date(props.post.publicDate);
    console.log(props.post.hasLike);

    return (
        <div className={cl.post}>
            <div className={cl.post_content}>
                <div className={cl.avatar_name}>
                    {props.post.user.avatar && <img src={`data:${props.post.user.avatar.contentType};base64,${props.post.user.avatar.data}`} height="400px" className={cl.avatar} />}
                    <div className={cl.username}>{props.post.user.name}</div>
                </div>

                <br></br>
                <p className={cl.post_body}>{props.post.body}</p>
                {props.post.mediaFiles && props.post.mediaFiles.map((mediaFile) =>
                    <img src={`data:${mediaFile.contentType};base64,${mediaFile.data}`} height="400px" key={mediaFile.id} />
                )}
                <div className={cl.post_btns}>
                    {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                    {props.post.hasLike ?
                        <HeartFilled style={{ fontSize: '40px', color: 'red' }} disabled={props.isLikeLoading} onClick={() => props.handleLike(props.post)} /> :
                        <HeartOutlined style={{ fontSize: '40px', color: 'red' }} disabled={props.isLikeLoading} onClick={() => props.handleLike(props.post)} />}
                    {/*<MyButton onClick={() => props.remove(props.post)}>Удалить</MyButton>*/}

                </div>
                {/*<div className={cl.post_data}>{Intl.DateTimeFormat('ru-RU', {
                    hour: '2-digit',
                    minute: '2-digit',
                    day: 'numeric',
                    month: 'numeric',
                    year: 'numeric',
                }).format(date)}</div>*/}
                {<div className={cl.post_data}>{format(date, "HH:mm d MMMM yyyy", { locale: ru })}</div>}
            </div>

        </div>
    );
};

export default CommentItem;