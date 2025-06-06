import { React, useState } from 'react';



const getPosts = (e) => {
    e.preventDefault();

    const posts = fetch("/api/Post", {
        method: "GET"
    }).then((response) => response.json());
    return posts;
}
