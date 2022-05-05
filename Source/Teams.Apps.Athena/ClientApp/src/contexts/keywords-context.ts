// <copyright file="keywords-context.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import IKeyword from "../models/keyword";

const keywordsContext = React.createContext([] as IKeyword[]);

export default keywordsContext;