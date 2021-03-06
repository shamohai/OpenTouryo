﻿//**********************************************************************************
//* Copyright (C) 2007,2016 Hitachi Solutions,Ltd.
//**********************************************************************************

#region Apache License
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

//**********************************************************************************
//* クラス名        ：WCFHTTPSvcForFx
//* クラス日本語名  ：WCFの.NETオブジェクトのバイナリ転送用
//*                   Soap Webメソッドを公開するサービス インターフェイス基盤。
//*
//* 作成日時        ：－
//* 作成者          ：生技
//* 更新履歴        ：
//* 
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  2012/12/14  西野 大介         新規作成
//*  2017/02/28  西野 大介         ExceptionDispatchInfoを取り入れた。
//**********************************************************************************

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.ExceptionServices;
using System.Diagnostics;

using Newtonsoft.Json.Linq;

using Touryo.Infrastructure.Framework.Transmission;
using Touryo.Infrastructure.Framework.Authentication;
using Touryo.Infrastructure.Framework.Exceptions;
using Touryo.Infrastructure.Framework.Common;
using Touryo.Infrastructure.Framework.Util;

using Touryo.Infrastructure.Public.Db;
using Touryo.Infrastructure.Public.IO;
using Touryo.Infrastructure.Public.Log;
using Touryo.Infrastructure.Public.Util;

namespace Touryo.Infrastructure.Framework.ServiceInterface.ASPNETWebService
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "WCFSvcForFx" を変更できます。

    /// <summary>
    /// WCFの.NETオブジェクトのバイナリ転送用Soap Webメソッドを公開するサービス インターフェイス基盤。
    /// </summary>
    [ServiceBehavior]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WCFHTTPSvcForFx : IWCFHTTPSvcForFx
    {
        #region グローバル変数

        /// <summary>インプロセス呼び出しの名前解決シングルトン クラス</summary>
        /// <remarks>
        /// 初期化は起動時の１回のみであり、
        /// 読み取り専用のデータを保持する場合
        /// のみに適用するデザインパターンとする。
        /// </remarks>
        private static InProcessNameService IPR_NS = new InProcessNameService();

        #endregion

        #region WCFの.NETオブジェクトのバイナリ転送用Soap Webメソッド

        /// <summary>
        /// WCFの.NETオブジェクトのバイナリ転送用Soap Webメソッド
        /// </summary>
        /// <param name="serviceName">サービス名</param>
        /// <param name="contextObject">コンテキスト</param>
        /// <param name="parameterValueObject">引数</param>
        /// <param name="returnValueObject">戻り値</param>
        /// <returns>返すべきエラーの情報</returns>
        /// <remarks>値は全て.NETオブジェクトをバイナリシリアライズしたバイト配列データ</remarks>
        public byte[] DotNETOnlineWS(
            string serviceName, ref byte[] contextObject,
            byte[] parameterValueObject, out byte[] returnValueObject)
        {
            // ステータス
            string status = "－";

            // 初期化のため
            returnValueObject = null;

            #region 呼出し制御関係の変数

            // アセンブリ名
            string assemblyName = "";

            // クラス名
            string className = "";

            #endregion

            #region 引数・戻り値関係の変数

            // コンテキスト情報
            object context; // 2009/09/29-この行

            // 引数・戻り値の.NETオブジェクト
            BaseParameterValue parameterValue = null;
            BaseReturnValue returnValue = null;

            // エラー情報（クライアント側で復元するため）
            WSErrorInfo wsErrorInfo = new WSErrorInfo();

            // エラー情報（ログ出力用）
            string errorType = ""; // 2009/09/15-この行
            string errorMessageID = "";
            string errorMessage = "";
            string errorToString = "";

            #endregion

            try
            {
                // 開始ログの出力
                LogIF.InfoLog("SERVICE-IF", FxLiteral.SIF_STATUS_START);

                #region 名前解決

                // ★
                status = FxLiteral.SIF_STATUS_NAME_SERVICE;

                // 名前解決（インプロセス）
                WCFHTTPSvcForFx.IPR_NS.NameResolution(serviceName, out assemblyName, out className);

                #endregion

                #region 引数のデシリアライズ

                // ★
                status = FxLiteral.SIF_STATUS_DESERIALIZE;

                // コンテキストクラスの.NETオブジェクト化
                context = BinarySerialize.BytesToObject(contextObject); // 2009/09/29-この行
                // ※ コンテキストの利用方法は任意だが、サービスインターフェイス上での利用に止める。

                // 引数クラスの.NETオブジェクト化
                parameterValue = (BaseParameterValue)BinarySerialize.BytesToObject(parameterValueObject);

                // 引数クラスをパラメタ セットに格納
                object[] paramSet = new object[] { parameterValue, DbEnum.IsolationLevelEnum.User };

                #endregion

                #region 認証処理のＵＯＣ

                // ★
                status = FxLiteral.SIF_STATUS_AUTHENTICATION;

                string access_token = (string)context;
                if (!string.IsNullOrEmpty(access_token))
                {
                    string sub = "";
                    List<string> roles = null;
                    List<string> scopes = null;
                    JObject jobj = null;

                    if (JwtToken.Verify(access_token, out sub, out roles, out scopes, out jobj))
                    {
                        // 認証成功
                        Debug.WriteLine("認証成功");
                    }
                    else
                    {
                        // 認証失敗（認証必須ならエラーにする。
                    }
                }
                else
                {
                    // 認証失敗（認証必須ならエラーにする。
                }

                //contextObject = BinarySerialize.ObjectToBytes(hogehoge); // 更新可能だが...。

                #endregion

                #region Ｂ層・Ｄ層呼出し

                // ★
                status = FxLiteral.SIF_STATUS_INVOKE;

                // #17-start
                try
                {
                    // Ｂ層・Ｄ層呼出し
                    //returnValue = (BaseReturnValue)Latebind.InvokeMethod(
                    //    AppDomain.CurrentDomain.BaseDirectory + "\\bin\\" + assemblyName + ".dll",
                    //    className, FxLiteral.TRANSMISSION_INPROCESS_METHOD_NAME, paramSet);
                    returnValue = (BaseReturnValue)Latebind.InvokeMethod(
                        assemblyName, className,
                        FxLiteral.TRANSMISSION_INPROCESS_METHOD_NAME, paramSet);
                }
                catch (System.Reflection.TargetInvocationException rtEx)
                {
                    //// InnerExceptionを投げなおす。
                    //throw rtEx.InnerException;

                    // スタックトレースを保って InnerException を throw
                    ExceptionDispatchInfo.Capture(rtEx.InnerException).Throw();
                }
                // #17-end

                #endregion

                #region 戻り値のシリアライズ

                // ★
                status = FxLiteral.SIF_STATUS_SERIALIZE;

                returnValueObject = BinarySerialize.ObjectToBytes(returnValue);

                #endregion

                // ★
                status = "";

                // 戻り値を返す。
                return null;
            }
            catch (BusinessSystemException bsEx)
            {
                // システム例外

                // エラー情報を設定する。
                wsErrorInfo.ErrorType = FxEnum.ErrorType.BusinessSystemException;
                wsErrorInfo.ErrorMessageID = bsEx.messageID;
                wsErrorInfo.ErrorMessage = bsEx.Message;

                // ログ出力用の情報を保存
                errorType = FxEnum.ErrorType.BusinessSystemException.ToString(); // 2009/09/15-この行
                errorMessageID = bsEx.messageID;
                errorMessage = bsEx.Message;

                errorToString = bsEx.ToString();

                // エラー情報を戻す。
                return BinarySerialize.ObjectToBytes(wsErrorInfo);
            }
            catch (FrameworkException fxEx)
            {
                // フレームワーク例外
                // ★ インナーエクセプション情報は消失

                // エラー情報を設定する。
                wsErrorInfo.ErrorType = FxEnum.ErrorType.FrameworkException;
                wsErrorInfo.ErrorMessageID = fxEx.messageID;
                wsErrorInfo.ErrorMessage = fxEx.Message;

                // ログ出力用の情報を保存
                errorType = FxEnum.ErrorType.FrameworkException.ToString(); // 2009/09/15-この行
                errorMessageID = fxEx.messageID;
                errorMessage = fxEx.Message;

                errorToString = fxEx.ToString();

                // エラー情報を戻す。
                return BinarySerialize.ObjectToBytes(wsErrorInfo);
            }
            catch (Exception ex)
            {
                // ログ出力用の情報を保存
                errorType = FxEnum.ErrorType.ElseException.ToString(); // 2009/09/15-この行
                errorMessageID = "－";
                errorMessage = ex.Message;

                errorToString = ex.ToString();

                throw; // SoapExceptionになって伝播
            }
            finally
            {
                // Sessionステートレス
                //HttpContext.Current.Session.Clear();
                //HttpContext.Current.Session.Abandon();

                // 終了ロクの出力
                if (status == "")
                {
                    // 終了ログ出力
                    LogIF.InfoLog("SERVICE-IF", "正常終了");
                }
                else
                {
                    // 終了ログ出力
                    LogIF.ErrorLog("SERVICE-IF",
                        "異常終了"
                        + "：" + status + "\r\n"
                        + "エラー タイプ：" + errorType + "\r\n" // 2009/09/15-この行
                        + "エラー メッセージID：" + errorMessageID + "\r\n"
                        + "エラー メッセージ：" + errorMessage + "\r\n"
                        + errorToString);
                }
            }
        }

        #endregion
    }
}
