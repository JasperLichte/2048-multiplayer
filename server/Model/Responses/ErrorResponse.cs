using server.model.interfaces;

namespace server.model.responses
{
    class ErrorResponse : IResponse
    {
        public ResponseTypes type;
        public string message;
        public ErrorResponse(string message)
        {
            this.type = ResponseTypes.ERROR;
            this.message=message;
        }
    }
}