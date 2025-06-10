import WeatherForecast from "../components/WeatherForecast";
import LogoutLink from "../components/LogoutLink";
import AuthorizeView, { AuthorizedUser } from "../components/AuthorizeView";
import PostIdPage from "./PostIdPage/PostIdPage";
import { useParams } from 'react-router';

function HomePostIdPage() {
    return (
        <AuthorizeView>
            <PostIdPage />
        </AuthorizeView>
    );
}

export default HomePostIdPage;
