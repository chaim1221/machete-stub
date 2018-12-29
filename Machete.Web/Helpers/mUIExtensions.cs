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
// ReSharper disable InconsistentNaming

namespace Machete.Web.Helpers
{
    public static class mUIExtensions
    {
        public static HtmlString mUIDropDownYesNoFor<TModel, TBool>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TBool>> expression, List<SelectListItem> yesNo, object attribs)
        {
//<div class="tb-field">
//    @Html.DropDownListFor(model => model.business, new SelectList(Model.def.yesnoSelectList(), "Value", "Text", Model), Shared.choose, new { tabindex = "2", id = idPrefix + "business" })
//    @Html.ValidationMessageFor(model => model.business)
//</div>
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, Html.ViewData, Html.MetadataProvider);
            var selectListItems = new SelectList(yesNo, "Value", "Text", metadata.Model);
            var dropDownList = Html.DropDownListFor(expression, selectListItems, Shared.choose, attribs);
            var validationMessage = Html.ValidationMessageFor(expression);
            var result = new HtmlString($"<div class=\"tb-field\">{dropDownList}{validationMessage}</div>");
            return result;
        }

        public static HtmlString mUIDropDownListFor<TModel, TSelect>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TSelect>> expression, SelectList list, object attribs)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, Html.ViewData, Html.MetadataProvider);
            var validationMessage = Html.ValidationMessageFor(expression);
            var selectListItems = new SelectList(list, "Value", "Text", metadata.Model);
            var dropDownList = Html.DropDownListFor(expression, selectListItems, Shared.choose, attribs);
            var result = new HtmlString($"<div class=\"tb-field\">{dropDownList}{validationMessage}</div>");
            return result;
        }

        public static HtmlString mUITableDropDownListFor<TModel, TSelect>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TSelect>> expression, SelectList list, object attribs)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, Html.ViewData, Html.MetadataProvider);
            var validationMessage = Html.ValidationMessageFor(expression);
            var selectListItems = new SelectList(list,"Value","Text", metadata.Model);
            var dropDownList = Html.DropDownListFor(expression, selectListItems, Shared.choose, attribs);
            var result = new HtmlString($"<div class=\"tb-field\">{dropDownList}{validationMessage}</div>");
            return result;
        }

        public static HtmlString mUITableLabelFor<TModel, TSelect>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TSelect>> expression)
        {
            //<div class="tb-label">
            //    @Html.LabelFor(expression)
            //</div>
            return new HtmlString($"<div class=\"tb-label\">{Html.LabelFor(expression)}</div>");
        }

        //11 usages in editor Worker.cshtml
        public static HtmlString mUITableTextBoxFor<TModel, TSelect>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TSelect>> expression, object attribs)
        {
            //<div class="tb-field">
            //    @Html.TextBoxFor(expression, attribs)
            //    @Html.ValidationMessageFor(expression)
            //</div>
            var textBox = Html.TextBoxFor(expression, attribs);
            var validationMessage = Html.ValidationMessageFor(expression);
            var result = new HtmlString($"<div class=\"tb-field\">{textBox}{validationMessage}</div>");
            return result;
        }

        public static HtmlString mUITableLabelAndTextBoxFor<TModel, TSelect>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TSelect>> expression, object attribs)
        {
            //<div class="tb-label">
            //    @Html.LabelFor(expression)
            //</div>
            //<div class="tb-field">
            //    @Html.TextBoxFor(expression, attribs)
            //    @Html.ValidationMessageFor(expression)
            //</div>
            var tableLabel = new HtmlString($"<div class=\"tb-label\">{Html.LabelFor(expression)}</div>");
            var textBox = Html.TextBoxFor(expression, attribs);
            var validationMessage = Html.ValidationMessageFor(expression);
            var tableTextBox = new HtmlString($"<div class=\"tb-field\">{textBox}{validationMessage}</div>");
            var result = new HtmlString(tableLabel + tableTextBox.ToString());
            return result;
        }


        public static HtmlString mUITableDateTextBoxFor<TModel, TSelect>(this IHtmlHelper<TModel> Html,
            Expression<Func<TModel, TSelect>> expression, object attribs)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, Html.ViewData, Html.MetadataProvider);
            var model = metadata.Model;
            var value = "";

            if ((DateTime?) model != DateTime.MinValue)
                value = model == null ? "" : ((DateTime) model).ToShortDateString();

            var htmlContent = Html.TextBox(metadata.Metadata.PropertyName, Html.Encode(value), attribs);

            var validationMessage = Html.ValidationMessageFor(expression);
            var result = new HtmlString($"<div class=\"tb-field\">{htmlContent}{validationMessage}</div>");
            return result;
        }
    }
}
