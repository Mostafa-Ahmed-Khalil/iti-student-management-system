using MediatR;
using ITI.SMS.Domain.Interfaces;

namespace ITI.SMS.Application.Courses.Commands;

public record DeleteCourseCommand(int Id) : IRequest;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
            throw new Common.Exceptions.NotFoundException($"Course with ID {request.Id} not found.");

        course.IsActive = false;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
