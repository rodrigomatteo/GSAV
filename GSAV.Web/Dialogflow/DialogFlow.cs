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
using GSAV.ServiceContracts.Interface;
using GSAV.Entity.Objects;

namespace GSAV.Web.Dialogflow
{
    public class DialogFlow
    {
        private readonly IBLSolicitud oIBLSolicitud;

        public DialogFlow()
        {
            
        }

        public DialogFlow(IBLSolicitud bLSolicitud)
        {
            oIBLSolicitud = bLSolicitud;
        }       

        public List<IntentoModel> ObtenerIntentos()
        {
            var intentos = new List<IntentoModel>();

            try
            {

                List<GSAV.Entity.Objects.Intencion> intenciones = oIBLSolicitud.ObtenerIntenciones().OneResult;


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

                        var row = intenciones.AsEnumerable().Where(q => q.IdDialogFlow.Equals(intento.Id)).FirstOrDefault();
                        if (row != null)
                        {
                            intento.FechaCreacion = row.StrFechaCreacion;
                            intento.IntencionPadre = row.IdPadreIntencion + string.Empty;
                        }

                        //Frases de Entrenamiento
                        var feId = 1;
                        foreach (var trainingPhrase in intent.TrainingPhrases)
                        {
                            var fraseEntrenamiento = new FraseEntrenamientoModel();
                            fraseEntrenamiento.Id = feId++;
                            fraseEntrenamiento.StrId = trainingPhrase.Name;

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

                    intentos = intentos.OrderBy(x => x.Nombre).ToList();

                }
            }
            catch (Exception ex)
            {

            }

            return intentos;
        }

        public  IntentoModel ObtenerIntento(string intentId)
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
                        IntentView = IntentView.Full
                    };


                    var intent = client.GetIntent(request);
                                                           
                    intento.Id = intent.IntentName.IntentId;
                    intento.Nombre = intent.DisplayName;
                    intento.FechaCreacion = oIBLSolicitud.ObtenerFechaIntencion(intento.Nombre).OneResult;

                    //Frases de Entrenamiento
                    var feId = 1;
                    foreach (var trainingPhrase in intent.TrainingPhrases)
                    {
                        var fraseEntrenamiento = new FraseEntrenamientoModel();
                        fraseEntrenamiento.Id = feId++;
                        fraseEntrenamiento.StrId = trainingPhrase.Name;
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

        public  List<FraseEntrenamientoModel> ObtenerFrasesEntrenamiento(string intentId)
        {
            var lista = new List<FraseEntrenamientoModel>();

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
                        IntentView = IntentView.Full
                    };


                    var intent = client.GetIntent(request);

                    //Frases de Entrenamiento
                    var feId = 1;
                    foreach (var trainingPhrase in intent.TrainingPhrases)
                    {
                        var fraseEntrenamiento = new FraseEntrenamientoModel();
                        fraseEntrenamiento.Id = feId++;
                        fraseEntrenamiento.StrId = trainingPhrase.Name;
                        fraseEntrenamiento.Tipo = trainingPhrase.Type + string.Empty;

                        foreach (var phrasePart in trainingPhrase.Parts)
                        {
                            fraseEntrenamiento.Descripcion = fraseEntrenamiento.Descripcion + " " + phrasePart.Text;
                        }
                        lista.Add(fraseEntrenamiento);
                    }                   
                }

                lista = lista.OrderBy(x => x.Descripcion).ToList();

            }
            catch (Exception ex)
            {

            }
            return lista;
        }

        public AlertModel UpdateIntent(Intencion intencion , List<FraseEntrenamientoModel> frases)
        {
            var intento = new IntentoModel();
            var resultado = new AlertModel();

            try
            {
                var fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow.json/") + ConstantesWeb.DialogFlow.FilePrivateKeyIdJson;

                if ((System.IO.File.Exists(fileSavePath)))
                {
                    var cred = GoogleCredential.FromFile(fileSavePath);

                    var channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());

                    var client = IntentsClient.Create(channel);

                    GetIntentRequest getRequest = new GetIntentRequest
                    {
                        IntentName = new IntentName(ConstantesWeb.DialogFlow.ProjectId, intencion.IdDialogFlow),
                        IntentView = IntentView.Full
                    };


                    var intent = client.GetIntent(getRequest);



                    //Actualizar Frases de Entrenamiento
                    intent.TrainingPhrases.Clear();                    
                    foreach (var frase_ in frases)
                    {
                        var trainingPhrasesParts = new List<string>();
                        trainingPhrasesParts.Add(frase_.Descripcion);
                        var phraseParts = new List<Intent.Types.TrainingPhrase.Types.Part>();
                        foreach (var part in trainingPhrasesParts)
                        {
                            phraseParts.Add(new Intent.Types.TrainingPhrase.Types.Part()
                            {
                                Text = part
                            });
                        }
                        var trainingPhrase = new Intent.Types.TrainingPhrase();
                        trainingPhrase.Parts.AddRange(phraseParts);
                        intent.TrainingPhrases.Add(trainingPhrase);
                    }

                    //Actualizar Respuesta
                    intent.Messages.Clear();
                    var text = new Intent.Types.Message.Types.Text();
                    text.Text_.Add(intencion.Respuesta);
                    var message_ = new Intent.Types.Message()
                    {
                        Text = text
                    };
                    intent.Messages.Add(message_);


                    UpdateIntentRequest updRequest = new UpdateIntentRequest
                    {
                        Intent = intent
                    };                    
                    Intent response = client.UpdateIntent(updRequest);


                    resultado.DisplayName = response.DisplayName;
                    resultado.Mensaje = "UPDATE-OK";
                }
            }
            catch (Exception ex)
            {
                resultado.MessageError = "ERROR: " + ex.Message;
                resultado.Mensaje = "UPDATE-ERROR";
            }

            return resultado;
        }

        public AlertModel CreateIntent(Intencion intencion, List<FraseEntrenamientoModel> frases)
        {
            var intento = new IntentoModel();
            var resultado = new AlertModel();

            try
            {
                var fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow.json/") + ConstantesWeb.DialogFlow.FilePrivateKeyIdJson;

                if ((System.IO.File.Exists(fileSavePath)))
                {
                    var cred = GoogleCredential.FromFile(fileSavePath);
                    var channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());
                    var client = IntentsClient.Create(channel);

                    var intent = new Intent();
                    intent.DisplayName = intencion.Nombre;


                    //Actualizar Frases de Entrenamiento
                    intent.TrainingPhrases.Clear();
                    foreach (var frase_ in frases)
                    {
                        var trainingPhrasesParts = new List<string>();
                        trainingPhrasesParts.Add(frase_.Descripcion);
                        var phraseParts = new List<Intent.Types.TrainingPhrase.Types.Part>();
                        foreach (var part in trainingPhrasesParts)
                        {
                            phraseParts.Add(new Intent.Types.TrainingPhrase.Types.Part()
                            {
                                Text = part
                            });
                        }
                        var trainingPhrase = new Intent.Types.TrainingPhrase();
                        trainingPhrase.Parts.AddRange(phraseParts);
                        intent.TrainingPhrases.Add(trainingPhrase);
                    }

                    //Actualizar Respuesta
                    intent.Messages.Clear();
                    var text = new Intent.Types.Message.Types.Text();
                    text.Text_.Add(intencion.Respuesta);
                    var message_ = new Intent.Types.Message()
                    {
                        Text = text
                    };
                    intent.Messages.Add(message_);


                    var newIntent = client.CreateIntent(
                        parent: new ProjectAgentName(ConstantesWeb.DialogFlow.ProjectId),
                        intent: intent
                    );
                    
                    resultado.Id = newIntent.IntentName.IntentId;
                    resultado.DisplayName = newIntent.DisplayName;              
                    oIBLSolicitud.InsertarIntencionConsulta(newIntent.DisplayName, newIntent.IntentName.IntentId, ConvertidorUtil.GmtToPacific(DateTime.Now), intencion.IntencionPadre);
                    resultado.Mensaje = "INSERT-OK";

                }
            }
            catch (Exception ex)
            {
                resultado.MessageError = "ERROR: " + ex.Message;
                resultado.Mensaje = "INSERT-ERROR";
            }

            return resultado;
        }

        public AlertModel DeleteIntent(Intencion intencion)
        {
            var intento = new IntentoModel();
            var resultado = new AlertModel();

            try
            {
                var fileSavePath = System.Web.HttpContext.Current.Server.MapPath("~/Dialogflow.json/") + ConstantesWeb.DialogFlow.FilePrivateKeyIdJson;

                if ((System.IO.File.Exists(fileSavePath)))
                {
                    var cred = GoogleCredential.FromFile(fileSavePath);
                    var channel = new Channel(SessionsClient.DefaultEndpoint.Host, SessionsClient.DefaultEndpoint.Port, cred.ToChannelCredentials());
                    var client = IntentsClient.Create(channel);
                                      

                    client.DeleteIntent(new IntentName(ConstantesWeb.DialogFlow.ProjectId, intencion.IdDialogFlow));

                  
                    resultado.Mensaje = "DELETE-OK";

                }
            }
            catch (Exception ex)
            {
                resultado.MessageError = "ERROR: " + ex.Message;
                resultado.Mensaje = "DELETE-ERROR";
            }

            return resultado;
        }
    }
}