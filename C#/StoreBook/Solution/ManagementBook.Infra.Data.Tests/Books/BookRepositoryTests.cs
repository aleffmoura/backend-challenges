namespace ManagementBook.Infra.Data.Tests.Books;

using FluentAssertions;
using LanguageExt;
using ManagementBook.Domain.Books;
using ManagementBook.Infra.Cross.Errors;
using ManagementBook.Infra.Data.Base;
using ManagementBook.Infra.Data.Features.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Data.SqlTypes;
using System.Threading.Tasks;

[TestFixture]
public class BookRepositoryTests
{
    private Mock<BookStoreContext> _bookStoreMock;
    private BookRepository _bookRepository;

    [SetUp]
    public void SetUp()
    {
        _bookStoreMock = new(new DbContextOptions<BookStoreContext>());
        _bookRepository = new(_bookStoreMock.Object);
    }

    [Test]
    public async Task BookRepositoryTests_GetAll_ShouldBeOk()
    {
        //arrange

        var booksCount = 1;
        List<Book> booksOnDb =
        [
            new Book
            {
                Id = Guid.NewGuid(),
                Author = "Author",
                Title = "Title",
                ReleaseDate = DateTime.Now,
            }
        ];
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Returns(booksOnDb.AsQueryable())
                      .Verifiable();

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.GetAll();

        //verifies
        result.Count().Should().Be(expected: booksCount);
        dbSetMock.Verify();
        _bookStoreMock.Verify();
    }
    [Test]
    public async Task BookRepositoryTests_GetAll_ShouldBeThrowNullReferenceException()
    {
        //action
        var action = async () => await _bookRepository.GetAll();

        //verifies
        await action.Should().ThrowAsync<NullReferenceException>();

        _bookStoreMock.Verify();
    }
    [Test]
    public async Task BookRepositoryTests_GetAll_ShouldBeSqlTruncateException()
    {
        //arrange
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Throws<SqlTruncateException>()
                      .Verifiable();

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var action = async () => await _bookRepository.GetAll();

        //verifies
        await action.Should().ThrowAsync<SqlTruncateException>();
        dbSetMock.Verify();
        _bookStoreMock.Verify();
    }

    [Test]
    public async Task BookRepositoryTests_GetById_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        Book bookOnDb = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.FindAsync(id))
                .ReturnsAsync(bookOnDb)
                .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.GetById(id);

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(b =>
        {
            b.Id.Should().Be(id);
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_GetById_ShouldBeNotFoundExceptionWithoutThrow()
    {
        //arrange
        var id = Guid.NewGuid();
        Book? bookOnDb = null;

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.FindAsync(id))
                .ReturnsAsync(bookOnDb)
                .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.GetById(id);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(b =>
        {
            b.Should().BeOfType<NotFoundError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_GetById_ShouldThrowSqlException_ButReturnsInternalError()
    {
        //arrange
        var id = Guid.NewGuid();

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.FindAsync(id))
                .Throws<SqlTruncateException>()
                .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.GetById(id);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(b =>
        {
            b.Should().BeOfType<InternalError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }

    [Test]
    public async Task BookRepositoryTests_Remove_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        Book bookOnDb = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.FindAsync(id))
                .ReturnsAsync(bookOnDb)
                .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();
        _bookStoreMock.Setup(s => s.Remove(bookOnDb))
                      .Verifiable();
        _bookStoreMock.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .Verifiable();

        //action
        var result = await _bookRepository.Remove(id);

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(_ =>
        {
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Remove_ShouldBeNotFoundError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book? bookOnDb = null;

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.FindAsync(id))
                .ReturnsAsync(bookOnDb)
                .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.Remove(id);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<NotFoundError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Remove_ShouldThrowError_ButReturnInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.FindAsync(id))
                .Throws<SqlTruncateException>()
                .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.Remove(id);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }

    [Test]
    public async Task BookRepositoryTests_Save_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.AddAsync(book, It.IsAny<CancellationToken>()))
                 .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        _bookStoreMock.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .Verifiable();

        //action
        var result = await _bookRepository.Save(book);

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(_ =>
        {
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Save_BookNull_ShouldBeInvalidObjectError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book? book = null;

        //action
        var result = await _bookRepository.Save(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InvalidObjectError>();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Save_BookOptionNone_ShouldBeInvalidObjectError()
    {
        //arrange
        var id = Guid.NewGuid();
        var book = Option<Book>.None;

        //action
        var result = await _bookRepository.Save(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InvalidObjectError>();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Save_AddAsyncThrowsException_ButReturnsInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.AddAsync(book, It.IsAny<CancellationToken>()))
                 .Throws<SqlTruncateException>()
                 .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.Save(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Save_ThrowsException_ButReturnsInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();
        dbSetMock.Setup(s => s.AddAsync(book, It.IsAny<CancellationToken>()))
                 .Verifiable();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        _bookStoreMock.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .Throws<SqlTruncateException>()
                      .Verifiable();

        //action
        var result = await _bookRepository.Save(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }

    [Test]
    public async Task BookRepositoryTests_Update_ShouldBeOk()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };
        Book bookOnDb = new()
        {
            Id = id,
            Author = "Author updated",
            Title = "Title updated",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        List<Book> booksOnDb = [bookOnDb];
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Returns(booksOnDb.AsQueryable())
                      .Verifiable();

        dbSetMock.Setup(s => s.Update(It.IsAny<Book>()))
                 .Verifiable();

        _bookStoreMock.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .Verifiable();

        //action
        var result = await _bookRepository.Update(book);

        //verifies
        result.IsSuccess.Should().BeTrue();
        result.IfSucc(_ =>
        {
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Update_FindAsyncNotFoundBook_ShouldBeNotFoundError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };

        List<Book> booksOnDb = [ ];
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Returns(booksOnDb.AsQueryable())
                      .Verifiable();

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        //action
        var result = await _bookRepository.Update(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<NotFoundError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Update_BookNull_ShouldBeInvalidObjectError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book? book = null;

        //action
        var result = await _bookRepository.Update(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InvalidObjectError>();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Update_SaveChangesAsyncThrowException_ButReturnInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };
        Book bookOnDb = new()
        {
            Id = id,
            Author = "Author updated",
            Title = "Title updated",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        List<Book> booksOnDb = [bookOnDb];
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Returns(booksOnDb.AsQueryable())
                      .Verifiable();

        dbSetMock.Setup(s => s.Update(It.IsAny<Book>()))
                 .Verifiable();

        _bookStoreMock.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .Throws<SqlTruncateException>()
                      .Verifiable();

        //action
        var result = await _bookRepository.Update(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
    [Test]
    public async Task BookRepositoryTests_Update_ThrowException_ButReturnInternalError()
    {
        //arrange
        var id = Guid.NewGuid();
        Book book = new()
        {
            Id = id,
            Author = "Author",
            Title = "Title",
            ReleaseDate = DateTime.Now,
        };
        Book bookOnDb = new()
        {
            Id = id,
            Author = "Author updated",
            Title = "Title updated",
            ReleaseDate = DateTime.Now,
        };

        var dbSetMock = new Mock<DbSet<Book>>();

        _bookStoreMock.SetupGet(bs => bs.Books)
                      .Returns(dbSetMock.Object)
                      .Verifiable();

        List<Book> booksOnDb = [bookOnDb];
        _bookStoreMock.Setup(bs => bs.AsNoTracking(It.IsAny<IQueryable<Book>>()))
                      .Returns(booksOnDb.AsQueryable())
                      .Verifiable();

        dbSetMock.Setup(s => s.Update(It.IsAny<Book>()))
                 .Throws<SqlTruncateException>()
                 .Verifiable();

        //action
        var result = await _bookRepository.Update(book);

        //verifies
        result.IsFaulted.Should().BeTrue();
        result.IfFail(fail =>
        {
            fail.Should().BeOfType<InternalError>();
            dbSetMock.Verify();
            _bookStoreMock.Verify();
        });
    }
}
