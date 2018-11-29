#region COPYRIGHT
// File:     mUIExtensions.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Machete.Web.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Machete.Web.Helpers
{
    public static class mUIExtensions
    {
        public static string tblabel = "<div class=\"tb-label\">";
        public static string tbdesclabel = "<div class=\"tb-label desc-label\">";
        public static string tbfield = "<div class=\"tb-field\">";
        public static string tbclose = "</div>";

        public static HtmlString mUIDropDownYesNoFor<TModel, TBool>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TBool>> expression,
             List<SelectListItem> yesNo,
            object attribs
            )
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            return new HtmlString(
                    tbfield +
                    htmlHelper.DropDownListFor(
                        expression,
                        new SelectList(yesNo,
                            "Value",
                            "Text",
                            metadata.Model),
                        Shared.choose,
                        attribs
                    ) +
                    htmlHelper.ValidationMessageFor(expression) +
                    tbclose
                    );
        }

        public static HtmlString mUIDropDownListFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            SelectList list,
            object attribs)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            return new HtmlString(
                tbfield +
                htmlHelper.DropDownListFor(
                    expression,
                    new SelectList(list,
                        "Value",
                        "Text",
                        metadata.Model),
                    Shared.choose,
                    attribs
                ) +
                htmlHelper.ValidationMessageFor(expression) +
                tbclose);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="list"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public static HtmlString mUITableDropDownListFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            SelectList list,
            object attribs)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            return new HtmlString(
                tbfield +
                htmlHelper.DropDownListFor(
                    expression,
                    new SelectList(list,
                                "Value",
                                "Text",
                                metadata.Model),
                    Shared.choose,
                    attribs
                ) +
                htmlHelper.ValidationMessageFor(expression) +
                tbclose
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static HtmlString mUITableLabelFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression)
        {
            return new HtmlString(
                tblabel +
                htmlHelper.LabelFor(expression) +
                tbclose
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static HtmlString mUITableDescLabelFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression)
        {
            return new HtmlString(
                tbdesclabel +
                htmlHelper.LabelFor(expression) +
                tbclose
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public static HtmlString mUITableTextBoxFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            var foo = htmlHelper.ValidationMessageFor(expression).ToString();
            return new HtmlString(
                tbfield +
                htmlHelper.TextBoxFor(expression, attribs) +
                htmlHelper.ValidationMessageFor(expression) +
                tbclose
                );
        }

        public static HtmlString mUITableDisplayFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TSelect>> expression,
    object attribs)
        {
            var foo = htmlHelper.ValidationMessageFor(expression).ToString();
            return new HtmlString(
                tbfield +
                htmlHelper.DisplayFor(expression, attribs) +
                htmlHelper.ValidationMessageFor(expression) +
                tbclose
                );
        }


        public static HtmlString mUITableLabelAndTextBoxFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            return new HtmlString(mUITableLabelFor(htmlHelper, expression)
                              + mUITableTextBoxFor(htmlHelper, expression, attribs)
                                      .ToString());
        }

        public static HtmlString mUITableLabelAndDisplayFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            return new HtmlString(mUITableLabelFor(htmlHelper, expression)
                              + mUITableDisplayFor(htmlHelper, expression, attribs)
                                  .ToString());
        }


        public static HtmlString mUITableDateTextBoxFor<TModel, TSelect>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            string value = String.Empty;
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            value = (DateTime?)metadata.Model == DateTime.MinValue ? "" : metadata.Model == null ? "" : ((DateTime)metadata.Model).ToShortDateString();

            return new HtmlString(
            tbfield +
                htmlHelper.TextBox(metadata.Metadata.PropertyName,
                    htmlHelper.Encode(value),
                    attribs) +
                htmlHelper.ValidationMessageFor(expression) +
            tbclose
                );
        }
    }
}

