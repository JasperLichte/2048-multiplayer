using server.model.interfaces;

namespace server.model.responses
{
    class ErrorResponse : IResponse
    {
        public ResponseTypes types;
        public string message;
        public ErrorResponse()
        {
            this.types = ResponseTypes.ERROR;
            this.message="Lorem Ipsum dolor si amet!";
        }
    }
}