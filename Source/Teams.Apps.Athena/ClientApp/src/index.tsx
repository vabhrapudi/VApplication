// <copyright file="index.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as ReactDOM from "react-dom";
import { BrowserRouter as Router } from "react-router-dom";
import App from "./app";

ReactDOM.render(
    <Router>
	    <App />
    </Router>, document.getElementById("root")
);
