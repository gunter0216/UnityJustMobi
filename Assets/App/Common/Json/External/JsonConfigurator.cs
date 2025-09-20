﻿using App.Common.Data.Runtime.Deserializer;
using App.Common.Data.Runtime.JsonLoader;
using App.Common.Data.Runtime.JsonSaver;
using App.Common.Data.Runtime.Serializer;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using Newtonsoft.Json;

namespace App.Common.Configurator.External
{
    [Configurator(ContextConstants.GlobalContext)]    
    public class JsonConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.Bind<IJsonLoader>().FromInstance(BeanJsonLoader());
            Container.Bind<IJsonSaver>().FromInstance(BeanJsonSaver());
            Container.Bind<IJsonDeserializer>().FromInstance(GetJsonDeserializer());
            Container.Bind<IJsonSerializer>().FromInstance(BeanJsonSerializer());
        }

        public JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "d.M.yyyy HH:mm:ss",
                Formatting = Formatting.Indented
            };
        }

        public IJsonLoader BeanJsonLoader()
        {
            return new DefaultJsonLoader(GetJsonDeserializer());
        }

        private IJsonSaver BeanJsonSaver()
        {
            return new DefaultJsonSaver(BeanJsonSerializer());
        }

        private IJsonDeserializer GetJsonDeserializer()
        {
            return new NewtonsoftJsonDeserializer(GetJsonSerializerSettings());
        }

        private IJsonSerializer BeanJsonSerializer()
        {
            return new NewtonsoftJsonSerializer(GetJsonSerializerSettings());
        }
    }
}