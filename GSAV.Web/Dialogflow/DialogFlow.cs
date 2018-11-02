using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Dialogflow.V2;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Grpc.Core;
using GSAV.Web.Models;
using GSAV.Web.Util;
using System.IO;

namespace GSAV.Web.Dialogflow
{
    public static class DialogFlow
    {
        public static List<IntentoModel> ObtenerIntentos()
        {
            var intentos = new List<IntentoModel>();

            try
            {
                var fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow.json/") + ConstantesWeb.DialogFlow.FilePrivateKeyIdJson;

                if ((System.IO.File.Exists(fileSavePath)))
                {
                    var cred = GoogleCredential.FromFile(fileSavePath);

                    var channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

                    var client = IntentsClient.Create(channel);

                    ListIntentsRequest request = new ListIntentsRequest
                    {
                        ParentAsProjectAgentName = new ProjectAgentName(ConstantesWeb.DialogFlow.ProjectId),
                        IntentView = IntentView.Full
                    };

                    var intents = client.ListIntents(request);

                    foreach (var intent in intents)
                    {
                        var intento = new IntentoModel();
                        intento.Id = intent.IntentName.IntentId;
                        intento.Nombre = intent.DisplayName;

                        //Frases de Entrenamiento
                        foreach (var trainingPhrase in intent.TrainingPhrases)
                        {
                            var fraseEntrenamiento = new FraseEntrenamientoModel();
                            fraseEntrenamiento.Id = trainingPhrase.Name;
                            foreach (var phrasePart in trainingPhrase.Parts)
                            {
                                fraseEntrenamiento.Descripcion = phrasePart.Text;
                            }
                            intento.FrasesEntrenamiento.Add(fraseEntrenamiento);
                        }

                        //Respuestas
                        foreach (var message in intent.Messages)
                        {
                            if (message.Text != null)
                            {
                                var idRespuesta = 0;
                                foreach (var text in message.Text.Text_)
                                {
                                    idRespuesta++;
                                    var respuesta = new RespuestaIntentoModel();
                                    respuesta.Id = idRespuesta + string.Empty;
                                    respuesta.Descripcion = text;
                                    intento.Respuestas.Add(respuesta);
                                }
                            }
                        }

                        intentos.Add(intento);

                    }

                }
            }
            catch (Exception ex)
            {

            }

            return intentos;
        }

        public static IntentoModel ObtenerIntento(string intentId)
        {
            var intento = new IntentoModel();

            try
            {
                var fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow.json/") + ConstantesWeb.DialogFlow.FilePrivateKeyIdJson;

                if ((System.IO.File.Exists(fileSavePath)))
                {
                    var cred = GoogleCredential.FromFile(fileSavePath);

                    var channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

                    var client = IntentsClient.Create(channel);

                    GetIntentRequest request = new GetIntentRequest
                    {
                        IntentName = new IntentName(ConstantesWeb.DialogFlow.ProjectId, intentId),
                    };


                    var intent = client.GetIntent(request);
                                                           
                    intento.Id = intent.IntentName.IntentId;
                    intento.Nombre = intent.DisplayName;

                    //Frases de Entrenamiento
                    foreach (var trainingPhrase in intent.TrainingPhrases)
                    {
                        var fraseEntrenamiento = new FraseEntrenamientoModel();
                        fraseEntrenamiento.Id = trainingPhrase.Name;
                        foreach (var phrasePart in trainingPhrase.Parts)
                        {
                            fraseEntrenamiento.Descripcion = phrasePart.Text;
                        }
                        intento.FrasesEntrenamiento.Add(fraseEntrenamiento);
                    }

                    //Respuestas
                    foreach (var message in intent.Messages)
                    {
                        if (message.Text != null)
                        {
                            var idRespuesta = 0;
                            foreach (var text in message.Text.Text_)
                            {
                                idRespuesta++;
                                var respuesta = new RespuestaIntentoModel();
                                respuesta.Id = idRespuesta + string.Empty;
                                respuesta.Descripcion = text;
                                intento.Respuestas.Add(respuesta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return intento;
        }

    }
}